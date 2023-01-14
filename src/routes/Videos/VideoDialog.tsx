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
          <form>
            <TextField
              value={video.title}
              label="Title"
              fullWidth
              margin="dense"
              id="title"
              variant="standard"
            />
            <TextField
              value={video.rating}
              label="Rating"
              fullWidth
              margin="dense"
              type="number"
              id="rating"
              variant="standard"
            />
            <Autocomplete
              multiple
              id="tags"
              options={['Disney', 'Pixar', 'Dessin animé', 'Film']}
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
          </form>
          <img />
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
