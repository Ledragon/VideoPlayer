import { Autocomplete, TextField } from '@mui/material';
import { useLocation } from 'react-router-dom';
import { Video } from '../../models/Video';

const VideoPage = (props: {}) => {
  const location = useLocation();
  
  const video = { title: '', rating: 0, tags: [] };
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

export default VideoPage;
