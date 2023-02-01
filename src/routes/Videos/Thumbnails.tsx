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
import { Video } from '../../models/Video';
import { useGenerateThumbnailsMutation } from '../../redux';
import getBase64Url from './getBase64Url';

interface PropType {
  open: boolean;
  video: Video | null;
  onCancel: () => void;
  //   onSave: (v: Video) => void;
}

const ThumbnailDialog = (props: PropType) => {
  const { open, video, onCancel } = props;
  const [count, setCount] = useState<number>(12);
  const [thumbnails, setThumbnails] = useState<{ id: number; data: string }[]>(
    []
  );
  const [selected, setSelected] = useState<{ id: number; data: string } | null>(
    null
  );
  const [isGeneratingThumbnails, setIsGeneratingThumbnails] =
    useState<boolean>(false);
  const [getThumbnails, status] = useGenerateThumbnailsMutation();
  console.log(status.data);
  useEffect(() => {
    setIsGeneratingThumbnails(status.isLoading);
  }, [status.isLoading]);

  useEffect(() => {
    setThumbnails((status.data || []).map((d, i) => ({ id: i, data: d })));
  }, [status.data]);

  const handleClick = () => {
    getThumbnails({
      filePath: video?.fileName || '',
      count: count
    });
    // setIsGeneratingThumbnails(true);
    // fetch(
    //   apiUrl +
    //     'thumbnails?' +
    //     new URLSearchParams({
    //       filePath: video?.fileName || '',
    //       count: count.toFixed()
    //     }),
    //   { mode: 'cors' }
    // )
    //   .then((data) => {
    //     return data.json();
    //   })
    //   .then((data) => {
    //     setThumbnails(data.map((d: string, i: number) => ({ id: i, data: d })));
    //     setIsGeneratingThumbnails(false);
    //   });
  };

  const handleSelect = (v: { id: number; data: string }) => {
    return setSelected(v);
  };

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
                      v.id === selected?.id
                        ? theme.palette.secondary.main
                        : null,
                    borderWidth: 1,
                    borderStyle: v.id === selected?.id ? 'solid' : 'none'
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
          {/* <Button onClick={() => onSave(video)}>Save</Button> */}
        </DialogActions>
      </Dialog>
    );
  } else {
    return <></>;
  }
};

export default ThumbnailDialog;
