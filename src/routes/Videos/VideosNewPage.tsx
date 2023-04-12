import { Box, LinearProgress, Paper, alpha } from '@mui/material';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Tag } from '../../models/Tag';
import { Video } from '../../models/Video';
import {
  useGetVideosQuery,
  useGetThumbnailsQuery,
  useGetTagsQuery,
  useGetTagVideosQuery,
  useUpdateVideoMutation,
  getVideoList,
  useAddThumbnailsMutation,
  getTagList
} from '../../redux';
import { RootState } from '../../redux/configureStore';
import Playlist from './Playlist';
import TagChips from './TagChips';
import VideoDialog from './VideoDialog';
import VideosImageList from './VideosImageList';

interface PropType {}
const VideosNewPage = (props: PropType) => {
  const [selectedVideo, setSelectedVideo] = useState<Video | null>(null);
  const [selectedTags, setSelectedTags] = useState<Tag[]>([]);
  const [filteredVideos, setFilteredVideos] = useState<Video[]>([]);

  const { isLoading, isFetching } = useGetVideosQuery();
  const { data: thumbnails, isLoading: isLoadingThumbnails } =
    useGetThumbnailsQuery();
  const { isLoading: isLoadingTags } = useGetTagsQuery();
  const { data: tagVideos, isFetching: isLoadingTagVideos } =
    useGetTagVideosQuery();
  const [updateVideo, result] = useUpdateVideoMutation();
  const [addThumbnails] = useAddThumbnailsMutation();

  const videos = useSelector((state: RootState) => getVideoList(state));
  const tags = useSelector((state: RootState) => getTagList(state));

  useEffect(() => {
    console.error(result.error);
  }, [result.error]);

  useEffect(() => {
    setFilteredVideos(videos || []);
  }, [videos]);

  const handleVideoClick = (video: Video) => {
    if (video) {
      setSelectedVideo(video);
    }
  };

  const handleTagClick = (tag: Tag) => {
    let newSelectedTags: Tag[];
    if (selectedTags.indexOf(tag) < 0) {
      newSelectedTags = selectedTags.concat([tag]);
    } else {
      newSelectedTags = selectedTags.map((d) => d);
      newSelectedTags.splice(selectedTags.indexOf(tag), 1);
    }
    setSelectedTags(newSelectedTags);
    if (newSelectedTags.length) {
      const st = newSelectedTags.map((d) => d.id);
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

  const isLoadingAny =
    isLoading ||
    isFetching ||
    isLoadingTagVideos ||
    isLoadingTags ||
    isLoadingThumbnails;
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
        sx={{ flex: '0 0 auto' }}
      >
        <TagChips
          selectedTags={selectedTags}
          tags={tags || []}
          onTagClick={handleTagClick}
        ></TagChips>
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
            overflowY: 'auto',
            flexGrow: 0,
            flexShrink: 0,
            p: 1
          }}
          id="videos-images-box"
        >
          <VideosImageList
            videos={filteredVideos}
            thumbnails={thumbnails || []}
            onClick={handleVideoClick}
          />
        </Box>
        <Box
          sx={{
            flexBasis: '20%',
            p: 1
          }}
        >
          <Paper
            sx={{
              height: '100%',
              backgroundColor: (theme) =>
                alpha(theme.palette.background.paper, 0.6)
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
          thumbnails={(thumbnails || []).filter(
            (t) => t.videoId === selectedVideo?.id
          )}
          video={selectedVideo}
          onSave={handleSave}
          onCancel={handleCancel}
          error={result.isError ? { message: result.error } : undefined}
        />
      ) : (
        <></>
      )}
    </Box>
  );
};

export default VideosNewPage;
