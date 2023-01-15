import {
  Dialog,
  DialogContent,
  DialogActions,
  Button,
  DialogTitle,
  TextField,
  Autocomplete,
  ImageList,
  ImageListItem
} from '@mui/material';
import { useState } from 'react';
import { Video } from '../../models/Video';
import getBase64Url from './getBase64Url';
import VideoCard from './VideoCard';

interface PropType {
  open: boolean;
  video: Video | null;
  onCancel: () => void;
  //   onSave: (v: Video) => void;
}

const ThumbnailDialog = (props: PropType) => {
  const { open, video, onCancel } = props;
  const [count, setCount] = useState<number>(12);
  const [thumbnails, setThumbnails] = useState<string[]>([]);
  console.table(thumbnails);
  const handleClick = () => {
    fetch(
      'http://localhost:5165/api/thumbnails?' +
        new URLSearchParams({
          filePath: video?.fileName || '',
          count: count.toFixed()
        }),
      { mode: 'cors' }
    )
      .then((data) => {
        return data.json();
      })
      .then((data) => {
        setThumbnails(data);
      });
  };

  if (video != null) {
    return (
      <Dialog open={open}>
        <DialogTitle>Generate thumbnails</DialogTitle>
        <DialogContent>
          <form>
            <TextField
              fullWidth
              label="Number of thumbnails"
              value={count}
              onChange={(e) => setCount(parseFloat(e.target.value))}
              type="number"
              margin="dense"
              variant="outlined"
            ></TextField>
            <Button onClick={() => handleClick()}>Generate</Button>
          </form>
          <ImageList cols={4} gap={6} sx={{ m: 1 }}>
            {thumbnails.map((v, i) => (
              <ImageListItem key={i}>
                <img src={getBase64Url(v)}></img>
              </ImageListItem>
            ))}
          </ImageList>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => onCancel()}>Cancel</Button>
          {/* <Button onClick={() => onSave(video)}>Save</Button> */}
        </DialogActions>
      </Dialog>
    );
  } else {
    return <></>;
  }
};

export default ThumbnailDialog;
