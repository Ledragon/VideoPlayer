import {
  Dialog,
  DialogContent,
  DialogActions,
  Button,
  DialogTitle,
  Box,
  CircularProgress,
  Typography,
  Alert,
  Skeleton,
  IconButton
} from '@mui/material';
import { useFormik } from 'formik';
import { useState } from 'react';
import NavigateBeforeIcon from '@mui/icons-material/NavigateBefore';
import NavigateAfterIcon from '@mui/icons-material/NavigateNext';
import PlaylistAddIcon from '@mui/icons-material/PlaylistAdd';

import { Tag } from '../../models/Tag';
import { TagVideo } from '../../models/TagVideo';
import { Thumbnail } from '../../models/Thumbnail';
import { Video } from '../../models/Video';
import {
  useGenerateThumbnailsMutation,
  useGetContactSheetForVideoQuery,
  useGetThumbnailsForVideoQuery
} from '../../redux';
import GeneratedThumbnails from './GeneratedThumbnails';
import ThumbnailsGenerator from './ThumbnailsGenerator';
import ThumbsImageList from './ThumbsImageList';
import VideoProperties from './VideoProperties';
import getBase64Url from './getBase64Url';
import ContactSheetForm from './ContactSheetForm';
import CreateTag from './CreateTag';
import { useDispatch } from 'react-redux';
import { addVideo } from '../../redux/playlist.slice';
import AddToPlaylist from './AddToPlaylist';

interface PropType {
  open: boolean;
  tags: Tag[];
  tagVideos: TagVideo[];
  video: Video;
  onCancel: () => void;
  onSave: (v: Video, thumbnails: string[]) => void;
  error?: { message: any };
  onPrevious?: () => void;
  onNext?: () => void;
}

const VideoDialog = (props: PropType) => {
  const {
    open,
    video,
    onSave,
    onCancel,
    tags,
    tagVideos,
    error,
    onPrevious,
    onNext
  } = props;
  const [_, status] = useGenerateThumbnailsMutation({
    fixedCacheKey: video.fileName
  });
  const [selectedThumbnails, setSelectedThumbnails] = useState<string[]>([]);
  const { data: contactSheet, isLoading } = useGetContactSheetForVideoQuery(
    video.id
  );
  const { data: thumbnails } = useGetThumbnailsForVideoQuery(video.id);
  const dispatch = useDispatch();

  const formik = useFormik({
    initialValues: {
      title: video.title,
      rating: video.rating || 0,
      tags: tagVideos
        ?.filter((tv) => tv.videoId === video.id)
        .map((tv) => tags.find((t) => t.id === tv.tagId))
        .filter((d) => !!d)
    },
    onSubmit: (value) => {
      const update = Object.assign({}, video, value);
      handleSave(update);
    },
    enableReinitialize: true
  });

  const handleCancel = () => {
    formik.resetForm();
    onCancel();
  };

  const handleSelectThumdbnail = (v: string) => {
    const index = selectedThumbnails.indexOf(v);
    if (index >= 0) {
      selectedThumbnails.splice(index, 1);
    } else {
      selectedThumbnails.push(v);
    }
    setSelectedThumbnails(selectedThumbnails.map((d) => d));
  };

  const handleSave = (v: Video) => {
    onSave(v, selectedThumbnails);
  };

  const handleBeforeClick = () => {
    if (onPrevious) {
      onPrevious();
    }
  };

  const handleNextClick = () => {
    if (onNext) {
      onNext();
    }
  };

  if (video != null) {
    return (
      <Dialog
        fullScreen={true}
        open={open}
      >
        <DialogTitle>
          <Box sx={{ display: 'flex' }}>
            <Box sx={{ flexGrow: 1 }}>{video.title}</Box>
            <Box>
              <IconButton onClick={handleBeforeClick}>
                <NavigateBeforeIcon></NavigateBeforeIcon>
              </IconButton>
              <IconButton onClick={handleNextClick}>
                <NavigateAfterIcon></NavigateAfterIcon>
              </IconButton>
            </Box>
          </Box>
        </DialogTitle>
        <DialogContent sx={{ display: 'flex', flexDirection: 'column' }}>
          <Box sx={{ display: 'flex', flexDirection: 'row', flexGrow: 1 }}>
            <Box
              sx={{
                flexBasis: '50%',
                display: 'flex',
                flexDirection: 'column'
              }}
            >
              <VideoProperties
                formik={formik}
                tags={tags}
                tagVideos={tagVideos}
                video={video}
                onSubmit={handleSave}
              />
              {isLoading ? (
                <Skeleton
                  variant="rounded"
                  sx={{
                    flexGrow: 1,
                    width: '100%'
                  }}
                />
              ) : null}
              {contactSheet && !isLoading ? (
                <Box
                  sx={{
                    flexGrow: 1
                  }}
                >
                  <img
                    src={getBase64Url(contactSheet.image)}
                    style={{}}
                  ></img>
                </Box>
              ) : (
                <></>
              )}
              <Box sx={{ display: 'flex' }}>
                <ContactSheetForm video={video} />
              </Box>
            </Box>
            <Box
              sx={{
                display: 'flex',
                flexDirection: 'column',
                flexBasis: '50%'
              }}
            >
              <Box>
                <CreateTag />
              </Box>
              <Box sx={{ flexBasis: '50%' }}>
                <ThumbsImageList thumbs={thumbnails || []} />
              </Box>
              <Box sx={{ flexBasis: '50%' }}>
                <ThumbnailsGenerator video={video} />
                {status.isLoading ? (
                  <CircularProgress />
                ) : (
                  <GeneratedThumbnails
                    data={status?.data || []}
                    selected={selectedThumbnails}
                    onSelect={handleSelectThumdbnail}
                  ></GeneratedThumbnails>
                )}
              </Box>
            </Box>
          </Box>

          <Box>
            {!!error ? (
              <Alert severity="error">
                An error occured. Check console for more information.
              </Alert>
            ) : null}
          </Box>
          <Box
            id="info-bar"
            sx={{ display: 'flex' }}
          >
            <Typography variant="caption">{video.fileName}</Typography>
          </Box>
        </DialogContent>
        <DialogActions>
          <AddToPlaylist video={video} />
          <Button
            variant="text"
            onClick={handleCancel}
          >
            Cancel
          </Button>
          <Button
            variant="text"
            disabled={
              (!formik.dirty && !selectedThumbnails.length) || !formik.isValid
            }
            onClick={formik.submitForm}
          >
            Save
          </Button>
        </DialogActions>
        {/* </form> */}
      </Dialog>
    );
  } else {
    return <></>;
  }
};

export default VideoDialog;
