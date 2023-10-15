import {
  Autocomplete,
  Box,
  IconButton,
  LinearProgress,
  Paper,
  TextField,
  alpha,
  FormControlLabel,
  Switch
} from '@mui/material';
import SortByAlphaOutlinedIcon from '@mui/icons-material/SortByAlphaOutlined';
import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';
import { useEffect, useMemo, useState } from 'react';
import { useSelector } from 'react-redux';
import { Tag } from '../../models/Tag';
import { Video } from '../../models/Video';
import {
  useGetVideosQuery,
  useGetTagsQuery,
  useGetTagVideosQuery,
  useUpdateVideoMutation,
  getVideoList,
  useAddThumbnailsMutation,
  getTagList
} from '../../redux';
import { RootState } from '../../redux/configureStore';
import Playlist from './Playlist';
import VideoDialog from './VideoDialog';
import VideosImageList from './VideosImageList';

interface PropType {}

enum SortOrder {
  AlphaAsc,
  AlphaDesc,
  DateAsc,
  DateDesc
}

const VideosNewPage = (props: PropType) => {
  const [selectedVideo, setSelectedVideo] = useState<Video | null>(null);
  const [selectedTags, setSelectedTags] = useState<Tag[]>([]);
  const [filteredVideos, setFilteredVideos] = useState<Video[]>([]);
  const [sortOrder, setSortOrder] = useState<SortOrder>(SortOrder.AlphaAsc);
  const [showMissingThumb, setShowMissingThumb] = useState(false);
  const [showMissingCS, setShowMissingCS] = useState(false);

  const { isLoading, isFetching } = useGetVideosQuery();
  const { isLoading: isLoadingTags } = useGetTagsQuery();
  const { data: tagVideos, isFetching: isLoadingTagVideos } =
    useGetTagVideosQuery();
  const [updateVideo, result] = useUpdateVideoMutation();
  const [addThumbnails] = useAddThumbnailsMutation();

  const videos = useSelector((state: RootState) => getVideoList(state));
  const tags = useSelector((state: RootState) => getTagList(state));

  useEffect(() => {
    if (result?.error) {
      console.error(result.error);
    }
  }, [result.error]);

  useEffect(() => {
    setFilteredVideos(videos || []);
    if (selectedVideo) {
      setSelectedVideo(videos?.find((v) => v.id === selectedVideo.id) || null);
    }
  }, [videos]);

  const handleVideoClick = (video: Video) => {
    if (video) {
      setSelectedVideo(video);
    }
  };

  const handleTagsChange = (_e: any, values: Tag[]) => {
    // let newSelectedTags: Tag[];
    // if (selectedTags.indexOf(tag) < 0) {
    //   newSelectedTags = selectedTags.concat([tag]);
    // } else {
    //   newSelectedTags = selectedTags.map((d) => d);
    //   newSelectedTags.splice(selectedTags.indexOf(tag), 1);
    // }
    setSelectedTags(values);
    if (values.length) {
      const st = values.map((d) => d.id);
      const ftv =
        tagVideos
          ?.filter((d) => st.indexOf(d.tagId) >= 0)
          .map((d) => d.videoId) || [];
      const fv = videos?.filter((v) => ftv.indexOf(v.id) >= 0);
      setFilteredVideos(fv || []);
    } else {
      setFilteredVideos(videos || []);
    }
  };

  const handleSave = (video: Video, thumbnails: string[]) => {
    Promise.all([
      updateVideo(video).unwrap(),
      addThumbnails(
        thumbnails.map((t) => ({ videoId: video.id, image: t, id: 0 }))
      ).unwrap()
    ]).then(() => setSelectedVideo(null));
  };

  const handleCancel = () => {
    setSelectedVideo(null);
  };

  const handlePreviousClick = () => {
    if (selectedVideo) {
      const index = filteredVideos.indexOf(selectedVideo);
      if (index > 0) {
        setSelectedVideo(filteredVideos[index - 1]);
      }
    }
  };

  const handleNextClick = () => {
    if (selectedVideo) {
      const index = filteredVideos.indexOf(selectedVideo);
      if (index < filteredVideos.length - 1) {
        setSelectedVideo(filteredVideos[index + 1]);
      }
    }
  };

  const handleAlphaSort = () => {
    setSortOrder(
      sortOrder === SortOrder.AlphaAsc
        ? SortOrder.AlphaDesc
        : SortOrder.AlphaAsc
    );
  };

  const handleDateSort = () => {
    setSortOrder(
      sortOrder === SortOrder.DateDesc ? SortOrder.DateAsc : SortOrder.DateDesc
    );
  };

  const handleThumbChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setShowMissingThumb(e.target.checked);
  };

  const handleCSChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setShowMissingCS(e.target.checked);
  };

  const displayedVideos = useMemo(() => {
    const toSort = filteredVideos.map(d => d);
    return toSort
      .sort((a, b) => {
        switch (sortOrder) {
          case SortOrder.AlphaAsc:
            return a.title > b.title ? 1 : -1;
          case SortOrder.AlphaDesc:
            return a.title < b.title ? 1 : -1;
          case SortOrder.DateAsc:
            return a.dateAdded > b.dateAdded ? 1 : -1;
          case SortOrder.DateDesc:
            return a.dateAdded < b.dateAdded ? 1 : -1;
        }
      });
  }, [sortOrder, filteredVideos, showMissingCS, showMissingThumb]);

  const isLoadingAny =
    isLoading || isFetching || isLoadingTagVideos || isLoadingTags;
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        p: 1,
        flexGrow: 1,
        flexShrink: 1,
        height: '100%'
      }}
    >
      {isLoadingAny ? (
        <Box>
          <LinearProgress></LinearProgress>
        </Box>
      ) : (
        <></>
      )}
      <Box
        id="tags-chips"
        sx={{ flex: '1 0 auto', m: 1, display: 'flex' }}
      >
        <Autocomplete
          sx={{ flexGrow: 1 }}
          multiple
          id="tags"
          options={
            tags.filter(
              (t) =>
                !tagVideos ||
                tagVideos.filter((d) => d.tagId === t.id).length > 0
            ) || []
          }
          getOptionLabel={(o) =>
            o
              ? `${o.value} (${
                  (tagVideos || []).filter((d) => d.tagId === o.id).length
                })`
              : ''
          }
          onChange={handleTagsChange}
          value={selectedTags}
          renderInput={(params) => (
            <TextField
              {...params}
              variant="standard"
              label="Tags"
              placeholder="Tags"
              margin="dense"
            />
          )}
        ></Autocomplete>
        <Box
          sx={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center'
          }}
        >
          <IconButton
            size="small"
            onClick={handleAlphaSort}
          >
            <SortByAlphaOutlinedIcon fontSize="small" />
          </IconButton>
          <IconButton
            size="small"
            onClick={handleDateSort}
          >
            <AccessTimeOutlinedIcon fontSize="small" />
          </IconButton>
          {/* <FormControlLabel
            control={
              <Switch
                size="small"
                checked={showMissingThumb}
                onChange={handleThumbChange}
              />
            }
            label="Thumb"
            sx={{ mx: 1 }}
          />
          <FormControlLabel
            control={
              <Switch
                size="small"
                checked={showMissingCS}
                onChange={handleCSChange}
              />
            }
            label="CS"
            sx={{ mx: 1 }}
          /> */}
        </Box>
      </Box>
      <Box
        sx={{
          display: 'flex',
          height: '100%',
          flex: '1 1 auto',
          overflow: 'hidden'
        }}
        id="central-part"
      >
        <Box
          sx={{
            flexBasis: '80%',
            flexGrow: 0,
            flexShrink: 0,
            flexDirection: 'column',
            display: 'flex'
          }}
        >
          <Box
            sx={{
              overflowY: 'auto',
              flexGrow: 1,
              flexShrink: 1,
              mx: 1
            }}
            id="videos-images-box"
          >
            <VideosImageList
              videos={displayedVideos}
              onClick={handleVideoClick}
            />
          </Box>
        </Box>
        <Box
          sx={{
            flexBasis: '20%',
            p: 0,
            overflow: 'hidden'
          }}
        >
          <Paper
            sx={{
              height: '100%',
              backgroundColor: (theme) =>
                alpha(theme.palette.background.paper, 0.6),
              overflowY: 'auto'
            }}
          >
            <Playlist />
          </Paper>
        </Box>
      </Box>
      {!!selectedVideo ? (
        <VideoDialog
          open={!!selectedVideo}
          tags={tags || []}
          tagVideos={(tagVideos || []).filter(
            (v) => v.videoId === selectedVideo?.id
          )}
          video={selectedVideo}
          onSave={handleSave}
          onCancel={handleCancel}
          error={result.isError ? { message: result.error } : undefined}
          onPrevious={handlePreviousClick}
          onNext={handleNextClick}
        />
      ) : (
        <></>
      )}
    </Box>
  );
};

export default VideosNewPage;
