import { Autocomplete, Button, ButtonGroup, Rating, TextField } from '@mui/material';
import { FormikProps } from 'formik';
import { Tag } from '../../models/Tag';
import { TagVideo } from '../../models/TagVideo';
import { Thumbnail } from '../../models/Thumbnail';
import { Video } from '../../models/Video';

const VideoProperties = (props: {
  video: Video;
  onSubmit: (updatedVideo: Video) => void;
  tags: Tag[];
  tagVideos?: TagVideo[];
  thumbnails?: Thumbnail[];
  formik: FormikProps<{ title: string; rating: number }>;
}) => {
  const { tags, formik } = props;

  return (
    <>
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
      <Rating
        value={formik.values.rating}
        // label="Rating"
        name="rating"
        onChange={formik.handleChange}
        // fullWidth
        // margin="dense"
        // type="number"
        id="rating"
        // variant="standard"
      />
      <Autocomplete
        multiple
        id="tags"
        options={tags || []}
        getOptionLabel={(o) => o.value}
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
    </>
  );
};

export default VideoProperties;
