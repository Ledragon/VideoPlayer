import { Box, CircularProgress, ImageList, ImageListItem } from '@mui/material';
import { useGenerateThumbnailsMutation } from '../../redux';
import getBase64Url from './getBase64Url';

interface PropType {
  data: string[];
}

const GeneratedThumbnails = (props: PropType) => {
  const { data } = props;
  return (
    <ImageList
      cols={4}
      gap={6}
      sx={{ m: 0.5 }}
    >
      {data.map((v, i) => (
        <ImageListItem
          key={i}
          sx={{}}
        >
          <img src={getBase64Url(v)}></img>
        </ImageListItem>
      ))}
    </ImageList>
  );
};

export default GeneratedThumbnails;
