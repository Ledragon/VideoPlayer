import { Autocomplete, Rating, TextField } from '@mui/material';
import { FormikProps } from 'formik';
import { Tag } from '../../models/Tag';
import { TagVideo } from '../../models/TagVideo';
import { Thumbnail } from '../../models/Thumbnail';
import { Video } from '../../models/Video';
import { useEffect, useState } from 'react';

const VideoProperties = (props: {
  video: Video;
  onSubmit: (updatedVideo: Video) => void;
  tags: Tag[];
  tagVideos?: TagVideo[];
  thumbnails?: Thumbnail[];
  formik: FormikProps<{
    title: string;
    rating: number;
    tags: (Tag | undefined)[];
  }>;
}) => {
  const { tags, tagVideos, formik, video } = props;

  const [usedTags, setUsedTags] = useState<(Tag | undefined)[]>([]);
  useEffect(() => {
    setUsedTags(
      tagVideos
        ?.filter((tv) => tv.videoId === video.id)
        .map((tv) => tags.find((t) => t.id === tv.tagId))
        .filter((t) => t !== undefined) || []
    );
  }, [tags, tagVideos, video]);

  const handleTagsChange = (
    _event: React.ChangeEvent<any>,
    value: any[]
  ) => {
    formik.setFieldValue('tags', value);
  };

  return (
    <form>
      <TextField
        fullWidth
        margin="dense"
        id="title"
        name="title"
        onChange={formik.handleChange}
        label="Title"
        value={formik.values.title}
        variant="standard"
        error={!!formik.errors.title}
        helperText={formik.errors.title}
      />
      <Rating
        value={formik.values.rating}
        name="rating"
        onChange={formik.handleChange}
        id="rating"
      />
      <Autocomplete
        multiple
        id="tags"
        options={tags || []}
        getOptionLabel={(o) => o?.value || ''}
        onChange={handleTagsChange}
        value={formik.values.tags}
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
