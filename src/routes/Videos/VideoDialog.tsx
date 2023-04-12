import {
  Dialog,
  DialogContent,
  DialogActions,
  Button,
  DialogTitle,
  Box,
  CircularProgress,
  Typography,
  Alert
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
import getBase64Url from './getBase64Url';
import ContactSheetForm from './ContactSheetForm';

interface PropType {
  open: boolean;
  tags: Tag[];
  tagVideos: TagVideo[];
  thumbnails: Thumbnail[];
  video: Video;
  onCancel: () => void;
  onSave: (v: Video, thumbnails: string[]) => void;
  error?: { message: any };
}

const VideoDialog = (props: PropType) => {
  const { open, video, onSave, onCancel, tags, tagVideos, thumbnails, error } =
    props;
  const [_, status] = useGenerateThumbnailsMutation({
    fixedCacheKey: video.fileName
  });
  const [selectedThumbnails, setSelectedThumbnails] = useState<string[]>([]);

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

  if (video != null) {
    return (
      <Dialog
        fullScreen={true}
        open={open}
      >
        <DialogTitle>{video.title}</DialogTitle>
        {/* <form onSubmit={formik.handleSubmit}> */}
        <DialogContent sx={{ display: 'flex', flexDirection: 'column' }}>
          <Box sx={{ display: 'flex', flexDirection: 'row', flexGrow: 1 }}>
            <Box sx={{ flexBasis: '50%' }}>
              <VideoProperties
                formik={formik}
                tags={tags}
                tagVideos={tagVideos}
                video={video}
                onSubmit={handleSave}
              />

              <img
                src={video.contactSheet ? getBase64Url(video.contactSheet) : ''}
                style={{
                  flexGrow: 1,
                  width: '100%'
                }}
              ></img>
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
          </Box>

          <Box>
            {!!error ? (
              <Alert severity="error">
                An error occured. Check console for more information.
              </Alert>
            ) : null}
          </Box>
          <Box>
            <Typography variant="caption">{video.fileName}</Typography>
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
