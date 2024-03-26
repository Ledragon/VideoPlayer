import {
  Box,
  IconButton,
  ImageList,
  ImageListItem,
  ImageListItemBar,
  SxProps
} from '@mui/material';
import DeleteForeverOutlinedIcon from '@mui/icons-material/DeleteForeverOutlined';
import { Thumbnail } from '../../models/Thumbnail';
import getBase64Url from './getBase64Url';
import { useDeleteThumbnailMutation } from '../../redux';
import { useState } from 'react';

interface PropType {
  cols?: number;
  sx?: SxProps
  thumbs: Thumbnail[];
}

const ThumbsImageList = (props: PropType) => {
  const { cols, sx, thumbs } = props;
  const [hovered, setHovered] = useState<number | undefined>(undefined);
  const [deleteThumbnail, deleteStatus] = useDeleteThumbnailMutation();

  const handleMouseEnter = (i: number) => {
    setHovered(i);
  };

  const handleMouseLeave = () => {
    setHovered(undefined);
  };

  return (
    <ImageList cols={cols ?? 4} gap={6} sx={{ ...sx, m: 0.5 }}>
      {thumbs.map((v, i) => (
        <ImageListItem key={i} sx={{}} onMouseEnter={() => handleMouseEnter(i)} onMouseLeave={handleMouseLeave}>
          <Box
            sx={{
              display: 'flex',
              flexGrow: 1,
              backgroundImage: v.image && `url('${getBase64Url(v.image)}')`,
              backgroundRepeat: 'no-repeat',
              backgroundSize: 'contain',
              backgroundPosition: 'center'
            }}></Box>

          {/* <img src={getBase64Url(v.image)}></img> */}
          {hovered === i ? (
            <ImageListItemBar
              position="top"
              actionIcon={
                <IconButton
                  onClick={() => {
                    deleteThumbnail(v.id);
                  }}>
                  <DeleteForeverOutlinedIcon />
                </IconButton>
              }></ImageListItemBar>
          ) : null}
        </ImageListItem>
      ))}
    </ImageList>
  );
};

export default ThumbsImageList;
