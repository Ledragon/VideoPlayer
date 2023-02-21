import {
  Dialog,
  DialogContent,
  DialogActions,
  Button,
  DialogTitle,
  ImageList,
  ImageListItem,
  CircularProgress
} from '@mui/material';
import { useState } from 'react';
import { Thumbnail } from '../../models/Thumbnail';
import { Video } from '../../models/Video';
import { useGenerateThumbnailsMutation } from '../../redux';
import getBase64Url from './getBase64Url';
import ThumbnailsGenerator from './ThumbnailsGenerator';
import ThumbsImageList from './ThumbsImageList';

interface PropType {
  open: boolean;
  tuple?: { video: Video; thumbnails: Thumbnail[] };
  onCancel: () => void;
  onSave: (thumbnails: Thumbnail[]) => void;
}

const ThumbnailDialog = (props: PropType) => {
  const { open, tuple, onCancel, onSave } = props;
  const [selected, setSelected] = useState<{ id: number; image: string }[]>([]);
  const [_, status] = useGenerateThumbnailsMutation();
  const { video, thumbnails: exisitingThumbs } = tuple || {
    video: null,
    thumbnails: []
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
      <Dialog
        open={open}
        maxWidth="lg"
        scroll="paper"
      >
        <DialogTitle>Generate thumbnails</DialogTitle>
        <DialogContent>
          <ThumbsImageList thumbs={exisitingThumbs || []} />
          <ThumbnailsGenerator video={video} />
          
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
