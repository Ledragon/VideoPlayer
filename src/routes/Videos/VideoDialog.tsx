import {
  Dialog,
  DialogContent,
  DialogActions,
  Button,
  DialogTitle,
  Box,
  CircularProgress,
  Typography
} from '@mui/material';
import { useFormik } from 'formik';
import { useState } from 'react';
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
  onSave: (v: Video, thumbnails: string[]) => void;
}

const VideoDialog = (props: PropType) => {
  const { open, video, onSave, onCancel, tags, tagVideos, thumbnails } = props;
  const [_, status] = useGenerateThumbnailsMutation({
    fixedCacheKey: video.fileName
  });
  const [selectedThumbnails, setSelectedThumbnails] = useState<string[]>([]);

  const formik = useFormik({
    initialValues: {
      title: video.title,
      rating: video.rating || 0
      // tags: tagVideos
      //   ?.filter((tv) => tv.videoId === video.id)
      //   .map((tv) => tags.find((t) => t.id === tv.tagId))
      //   .filter((d) => !!d)
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

  if (video != null) {
    return (
      <Dialog
        fullScreen
        open={open}
      >
        <DialogTitle>{video.title}</DialogTitle>
        <form onSubmit={formik.handleSubmit}>
          <DialogContent>
            <VideoProperties
              formik={formik}
              tags={tags}
              video={video}
              onSubmit={handleSave}
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
                    selected={selectedThumbnails}
                    onSelect={handleSelectThumdbnail}
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
              disabled={!formik.dirty || !formik.isValid}
            >
              Save
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    );
  } else {
    return <></>;
  }
};

export default VideoDialog;
