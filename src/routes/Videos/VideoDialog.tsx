import CloseFullscreenOutlinedIcon from '@mui/icons-material/CloseFullscreenOutlined';
import NavigateBeforeIcon from '@mui/icons-material/NavigateBefore';
import NavigateAfterIcon from '@mui/icons-material/NavigateNext';
import OpenInFullOutlinedIcon from '@mui/icons-material/OpenInFullOutlined';
import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, IconButton, Tab, Tabs } from '@mui/material';
import { useFormik } from 'formik';
import { useMemo, useState } from 'react';

import { Tag } from '../../models/Tag';
import { TagVideo } from '../../models/TagVideo';
import { Video } from '../../models/Video';
import AddToPlaylist from './AddToPlaylist';
import ContactSheet from './ContactSheet';
import ThumbsEdition from './ThumbsEdition';
import VideoEdtion from './VideoEdition';
import VideoInfos from './VideoInfos';

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
  const { open, video, onSave, onCancel, tags, tagVideos, error, onPrevious, onNext } = props;
  const [selectedThumbnails, setSelectedThumbnails] = useState<string[]>([]);
  const [selectedTab, setSelectedTab] = useState<number>(0);
  const [isFullScreen, setIsFullScreen] = useState(true);

  const tagsForVideo = useMemo(() => {
    return tagVideos
      ?.filter((tv) => tv.videoId === video.id)
      .map((tv) => tags.find((t) => t.id === tv.tagId))
      .filter((d) => !!d);
  }, [video, tagVideos, tags]);

  const formik = useFormik({
    initialValues: {
      title: video.title,
      rating: video.rating || 0,
      tags: tagsForVideo
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
      <Dialog fullScreen={isFullScreen} open={open} fullWidth maxWidth="lg">
        <DialogTitle>
          <Box sx={{ display: 'flex' }}>
            <Box sx={{ flexGrow: 1 }}>{video.title}</Box>
            <Box>
              <IconButton onClick={() => setIsFullScreen(!isFullScreen)}>
                {isFullScreen && <CloseFullscreenOutlinedIcon />}
                {!isFullScreen && <OpenInFullOutlinedIcon />}
              </IconButton>
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
          <Box sx={{ display: 'flex', flexGrow: 1, flexDirection: 'row', overflow: 'auto' }}>
            <Box sx={{ flexBasis: '30%', flexGrow: 0, flexShrink: 0, display: 'flex', flexDirection: 'column' }}>
              <Tabs value={selectedTab} onChange={(e, v) => setSelectedTab(v)}>
                <Tab label="Properties" />
                <Tab label="Edition" />
                <Tab label="Thumbs" />
              </Tabs>
              <Box sx={{ flexGrow: 1, overflow: 'auto' }}>
                <Box sx={{ height: '100%', display: 'flex', flexDirection: 'column', overflow: 'auto' }}>
                  {selectedTab === 0 && <VideoInfos tags={tagsForVideo} video={video} />}
                  {selectedTab === 1 && (
                    <VideoEdtion
                      error={error}
                      formik={formik}
                      tags={tags}
                      tagVideos={tagVideos}
                      video={video}
                      onSave={handleSave}
                      selectedThumbnails={selectedThumbnails}
                      setSelectedThumbnails={setSelectedThumbnails}
                    />
                  )}
                  {selectedTab === 2 && <ThumbsEdition video={video} selectedThumbnails={selectedThumbnails} setSelectedThumbnails={setSelectedThumbnails} />}
                </Box>
              </Box>
            </Box>
            <Box sx={{ p: 1, display: 'flex', flexDirection: 'column', flexGrow: 1, flexBasis: 'auto', overflow: 'hidden' }} className="center">
              <ContactSheet id={video.id}></ContactSheet>
            </Box>
          </Box>
        </DialogContent>
        <DialogActions>
          <AddToPlaylist video={video} />
          <Button variant="text" onClick={handleCancel}>
            Cancel
          </Button>
          <Button variant="text" disabled={(!formik.dirty && !selectedThumbnails.length) || !formik.isValid} onClick={formik.submitForm}>
            Save
          </Button>
        </DialogActions>
      </Dialog>
    );
  } else {
    return <></>;
  }
};

export default VideoDialog;
