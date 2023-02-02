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
  video: Video | null;
  onCancel: () => void;
  onSave: (thumbnails: Thumbnail[]) => void;
}

const ThumbnailDialog = (props: PropType) => {
  const { open, video, onCancel, onSave } = props;
  const [count, setCount] = useState<number>(12);
  const [selected, setSelected] = useState<{ id: number; data: string }[]>(
    []
  );
  const [getThumbnails, status] = useGenerateThumbnailsMutation();

  const handleClick = () => {
    getThumbnails({
      filePath: video?.fileName || '',
      count: count
    });
  };

  const thumbnails =
    status.isSuccess && status.data
      ? status.data.map((d, i) => ({ id: i, data: d }))
      : [];

  const handleSelect = (v: { id: number; data: string; }) => {
    const current = selected.filter(s => s.id !== v.id).concat([v]);
    return setSelected(current);
  };

  const handleSave = () => {
    if (!!video) {
      onSave(selected.map(d => ({ id: 0, image: d.data, videoId: video.id })));
    }
  }

  if (video != null) {
    return (
      <Dialog open={open} maxWidth="lg" scroll="paper">
        <DialogTitle>Generate thumbnails</DialogTitle>
        <DialogContent>
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
                    src={getBase64Url(v.data)}
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
