import {
  Dialog,
  DialogContent,
  DialogActions,
  Button,
  DialogTitle,
  Box,
  CircularProgress
} from '@mui/material';
import { useFormik } from 'formik';
import { Tag } from '../../models/Tag';
import { TagVideo } from '../../models/TagVideo';
import { Thumbnail } from '../../models/Thumbnail';
import { Video } from '../../models/Video';
import { useGenerateThumbnailsMutation } from '../../redux';
import GeneratedThumbnails from './GeneratedThumbnails';
import ThumbnailsGenerator from './ThumbnailsGenerator';
import ThumbsImageList from './ThumbsImageList';
import VideoProperties from './VideoProperties';

interface PropType {
  open: boolean;
  tags: Tag[];
  tagVideos: TagVideo[];
  thumbnails: Thumbnail[];
  video: Video;
  onCancel: () => void;
  onSave: (v: Video) => void;
}

const VideoDialog = (props: PropType) => {
  const { open, video, onSave, onCancel, tags, tagVideos, thumbnails } = props;
  const [_, status] = useGenerateThumbnailsMutation({
    fixedCacheKey: video.fileName
  });
  const formik = useFormik({
    initialValues: {
      title: video.title,
      rating: video.rating || 0
    },
    onSubmit: (value) => {
      const update = Object.assign({}, video, value);
      onSave(update);
    },
    enableReinitialize: true
  });

  const handleSave = () => {
    if (onSave && video) {
      onSave(video);
    }
  };

  const handleCancel = () => {
    formik.resetForm();
    onCancel();
  };

  if (video != null) {
    return (
      <form onSubmit={formik.handleSubmit}>
        <Dialog
          fullScreen
          open={open}
        >
          <DialogTitle>{video.title}</DialogTitle>
          <DialogContent>
            <VideoProperties
              formik={formik}
              tags={tags}
              video={video}
              onSubmit={onSave}
            />
            <Box sx={{ display: 'flex' }}>
              <Box sx={{ flexBasis: '50%' }}>
                <ThumbsImageList thumbs={thumbnails} />
              </Box>
              <Box sx={{ flexBasis: '50%' }}>
                <ThumbnailsGenerator video={video} />
                {status.isLoading ? (
                  <CircularProgress />
                ) : (
                  <GeneratedThumbnails
                    data={status?.data || []}
                  ></GeneratedThumbnails>
                )}
              </Box>
            </Box>
          </DialogContent>
          <DialogActions>
            <Button
              variant="text"
              onClick={handleCancel}
            >
              Cancel
            </Button>
            <Button
              variant="text"
              type="submit"
            >
              Save
            </Button>
          </DialogActions>
        </Dialog>
      </form>
    );
  } else {
    return <></>;
  }
};

export default VideoDialog;
