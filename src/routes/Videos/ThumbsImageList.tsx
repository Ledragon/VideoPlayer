import DeleteForeverOutlinedIcon from '@mui/icons-material/DeleteForeverOutlined';
import { Box, IconButton, ImageList, ImageListItem, ImageListItemBar, SxProps } from '@mui/material';
import { useState } from 'react';

import { Thumbnail } from '../../models/Thumbnail';
import { useDeleteThumbnailMutation } from '../../redux';
import getBase64Url from './getBase64Url';

interface PropType {
  cols?: number;
  sx?: SxProps;
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
        <ImageListItem key={i} onMouseEnter={() => handleMouseEnter(i)} onMouseLeave={handleMouseLeave}>
          <Box
            sx={{
              flexGrow: 1,
              backgroundImage: `url('${getBase64Url(v.image)}')`,
              height: '120px',
              backgroundRepeat: 'no-repeat',
              backgroundSize: 'contain',
              backgroundPosition: 'center'
            }}
          />
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
