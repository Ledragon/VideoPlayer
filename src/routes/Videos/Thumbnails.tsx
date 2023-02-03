import {
  Dialog,
  DialogContent,
  DialogActions,
  Button,
  DialogTitle,
  TextField,
  Autocomplete,
  ImageList,
  ImageListItem,
  Backdrop,
  CircularProgress
} from '@mui/material';
import { useEffect, useState } from 'react';
import { Thumbnail } from '../../models/Thumbnail';
import { Video } from '../../models/Video';
import { useGenerateThumbnailsMutation } from '../../redux';
import getBase64Url from './getBase64Url';

interface PropType {
  open: boolean;
  tuple?: { video: Video; thumbnails: Thumbnail[] };
  onCancel: () => void;
  onSave: (thumbnails: Thumbnail[]) => void;
}

const ThumbnailDialog = (props: PropType) => {
  const { open, tuple, onCancel, onSave } = props;
  const [count, setCount] = useState<number>(12);
  const [selected, setSelected] = useState<{ id: number; image: string }[]>([]);
  const [getThumbnails, status] = useGenerateThumbnailsMutation();
  const { video, thumbnails: exisitingThumbs } = tuple || {
    video: null,
    thumbnails: []
  };

  const handleClick = () => {
    getThumbnails({
      filePath: video?.fileName || '',
      count: count
    });
  };

  const thumbnails =
    status.isSuccess && status.data
      ? status.data.map((d, i) => ({ id: i, image: d }))
      : [];

  const handleSelect = (v: { id: number; image: string }) => {
    const current = selected.filter((s) => s.id !== v.id).concat([v]);
    return setSelected(current);
  };

  const handleSave = () => {
    if (!!video) {
      onSave(
        selected.map((d) => ({ id: 0, image: d.image, videoId: video.id }))
      );
    }
  };

  if (video != null) {
    return (
      <Dialog open={open} maxWidth="lg" scroll="paper">
        <DialogTitle>Generate thumbnails</DialogTitle>
        <DialogContent>
          <ImageList cols={4} gap={6} sx={{ m: 0.5 }}>
            {exisitingThumbs.map((v, i) => (
              <ImageListItem key={i} sx={{}}>
                <img src={getBase64Url(v.image)}></img>
              </ImageListItem>
            ))}
          </ImageList>
          <form style={{ display: 'flex', alignItems: 'center' }}>
            <TextField
              label="Number of thumbnails"
              value={count}
              onChange={(e) => setCount(parseFloat(e.target.value))}
              type="number"
              margin="dense"
              variant="outlined"
              size="small"
              sx={{ flexGrow: 1 }}
            ></TextField>
            <Button
              onClick={() => handleClick()}
              variant="outlined"
              sx={{ m: 2 }}
            >
              Generate
            </Button>
          </form>
          {status.isLoading ? (
            <CircularProgress />
          ) : (
            <ImageList cols={4} gap={6} sx={{ m: 0.5 }}>
              {thumbnails.map((v, i) => (
                <ImageListItem
                  key={i}
                  sx={{
                    borderColor: (theme) =>
                      selected.map((d) => d.id).indexOf(v.id) > 0
                        ? theme.palette.secondary.main
                        : null,
                    borderWidth: 1,
                    borderStyle:
                      selected.map((d) => d.id).indexOf(v.id) > 0
                        ? 'solid'
                        : 'none'
                  }}
                >
                  <img
                    src={getBase64Url(v.image)}
                    onClick={(e) => handleSelect(v)}
                  ></img>
                </ImageListItem>
              ))}
            </ImageList>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={() => onCancel()}>Cancel</Button>
          <Button onClick={() => handleSave()}>Save</Button>
        </DialogActions>
      </Dialog>
    );
  } else {
    return <></>;
  }
};

export default ThumbnailDialog;
