import { Autocomplete, TextField } from '@mui/material';
import { Video } from '../../models/Video';

const VideoProperties = (props: { video: Video }) => {
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
      <TextField
        value={video.category}
        label="category"
        fullWidth
        margin="dense"
        type="string"
        id="category"
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
  );
};

export default VideoProperties;
