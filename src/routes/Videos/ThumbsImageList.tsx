import {
  Box,
  IconButton,
  ImageList,
  ImageListItem,
  ImageListItemBar
} from '@mui/material';
import DeleteForeverOutlinedIcon from '@mui/icons-material/DeleteForeverOutlined';
import { Thumbnail } from '../../models/Thumbnail';
import getBase64Url from './getBase64Url';
import { useDeleteThumbnailMutation } from '../../redux';

interface PropType {
  thumbs: Thumbnail[];
}

const ThumbsImageList = (props: PropType) => {
  const { thumbs } = props;
  const [deleteThumbnail, deleteStatus] = useDeleteThumbnailMutation();
  return (
    <ImageList
      cols={4}
      gap={6}
      sx={{ m: 0.5 }}
    >
      {thumbs.map((v, i) => (
        <ImageListItem
          key={i}
          sx={{}}
        >
          <img src={getBase64Url(v.image)}></img>
          <ImageListItemBar
            position="top"
            actionIcon={
              <IconButton
                onClick={() => {
                  deleteThumbnail(v.id);
                }}
              >
                <DeleteForeverOutlinedIcon />
              </IconButton>
            }
          ></ImageListItemBar>
        </ImageListItem>
      ))}
    </ImageList>
  );
};

export default ThumbsImageList;
