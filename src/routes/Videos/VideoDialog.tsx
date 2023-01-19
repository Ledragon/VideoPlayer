import {
  Dialog,
  DialogContent,
  DialogActions,
  Button,
  DialogTitle,
  TextField,
  Autocomplete
} from '@mui/material';
import { Video } from '../../models/Video';
import VideoCard from './VideoCard';
import VideoProperties from './VideoProperties';

interface PropType {
  open: boolean;
  video: Video | null;
  onCancel: () => void;
  onSave: (v: Video) => void;
}

const VideoDialog = (props: PropType) => {
  const { open, video, onSave, onCancel } = props;
  if (video != null) {
    return (
      <Dialog fullScreen open={open}>
        <DialogTitle>{video.title}</DialogTitle>
        <DialogContent>
          <VideoProperties video={video} onSubmit={onSave}/>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => onCancel()}>Cancel</Button>
          <Button onClick={() => onSave(video)}>Save</Button>
        </DialogActions>
      </Dialog>
    );
  } else {
    return <></>;
  }
};

export default VideoDialog;
