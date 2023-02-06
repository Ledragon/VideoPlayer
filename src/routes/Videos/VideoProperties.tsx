import {
  Autocomplete,
  Button,
  ButtonGroup,
  ImageList,
  ImageListItem,
  TextField
} from '@mui/material';
import { useFormik } from 'formik';
import { Thumbnail } from '../../models/Thumbnail';
import { Video } from '../../models/Video';
import getBase64Url from './getBase64Url';

const VideoProperties = (props: {
  video: Video;
  onSubmit: (updatedVideo: Video) => void;
  thumbnails?: Thumbnail[];
}) => {
  const { onSubmit, video, thumbnails } = props;
  const formik = useFormik({
    initialValues: {
      title: video.title,
      rating: video.rating || 0,
      category: video.category || ''
    },
    onSubmit: (value) => {
      const update = Object.assign({}, video, value);
      onSubmit(update);
    },
    enableReinitialize: true
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
      <ImageList cols={2}>
        {(thumbnails || []).map((t) => (
          <ImageListItem key={t.id}>
            <img src={getBase64Url(t.image)}></img>
          </ImageListItem>
        ))}
      </ImageList>
      <ButtonGroup>
        <Button variant="text" onClick={() => formik.resetForm()}>
          Cancel
        </Button>
        <Button variant="text" type="submit">
          Save
        </Button>
      </ButtonGroup>
    </form>
  );
};

export default VideoProperties;
