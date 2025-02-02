import { Button, TextField } from '@mui/material';
import { ChangeEvent, useState } from 'react';

import { Video } from '../../models/Video';
import { useGenerateThumbnailsMutation } from '../../redux';

interface PropType {
  video: Video;
}

const ThumbnailsGenerator = (props: PropType) => {
  const { video } = props;

  const [count, setCount] = useState<number>(12);
  const [getThumbnails, status] = useGenerateThumbnailsMutation({ fixedCacheKey: video.fileName });
  const [width, setWidth] = useState<number>(640);

  const handleClick = () => {
    getThumbnails({
      filePath: video?.fileName || '',
      count: count,
      width
    });
  };
  const handleCountChange = (e: ChangeEvent<any>) => setCount(parseFloat(e.target.value));

  return (
    <form style={{ display: 'flex', alignItems: 'center' }}>
      <TextField
        label="Number of thumbnails"
        value={count}
        onChange={handleCountChange}
        type="number"
        margin="dense"
        variant="outlined"
        size="small"
        sx={{ flexGrow: 1 }}></TextField>
      <TextField
        label="Width"
        value={width}
        onChange={(e) => setWidth(parseInt(e.target.value))}
        type="number"
        margin="dense"
        variant="outlined"
        size="small"
        sx={{ flexGrow: 1 }}></TextField>
      <Button onClick={handleClick} variant="outlined" sx={{ m: 2 }} disabled={status.isLoading}>
        Generate
      </Button>
    </form>
  );
};

export default ThumbnailsGenerator;
