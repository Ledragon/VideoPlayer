import { Autocomplete, TextField } from '@mui/material';
import { Video } from '../../models/Video';

const VideoForm = (props: { video: Video }) => {
  const { video } = props;
  return (
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
        value={video.tags}
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
  );
};

export default VideoForm;
