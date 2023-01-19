import { Autocomplete, Button, TextField } from '@mui/material';
import { Box } from '@mui/system';
import { useFormik } from 'formik';
import { Video } from '../../models/Video';

const VideoProperties = (props: {
  video: Video;
  onSubmit: (updatedVideo: Video) => void;
}) => {
  const { onSubmit, video } = props;
  const formik = useFormik({
    initialValues: {
      title: video.title,
      rating: video.rating,
      category: video.category
    },
    onSubmit: (value) => {
      const update = Object.assign({}, video, value);
      onSubmit(update);
    }
  });
  return (
    <form onSubmit={formik.handleSubmit}>
      <TextField
        fullWidth
        margin="dense"
        id="title"
        name="title"
        onChange={formik.handleChange}
        label="Title"
        value={formik.values.title}
        variant="standard"
      />
      <TextField
        value={formik.values.rating}
        label="Rating"
        name="rating"
        onChange={formik.handleChange}
        fullWidth
        margin="dense"
        type="number"
        id="rating"
        variant="standard"
      />
      <TextField
        value={formik.values.category}
        onChange={formik.handleChange}
        label="Category"
        name="category"
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
      <Box>
        <Button variant="text" onClick={() => formik.resetForm()}>
          Cancel
        </Button>
        <Button variant="text" type="submit">
          Save
        </Button>
      </Box>
    </form>
  );
};

export default VideoProperties;
