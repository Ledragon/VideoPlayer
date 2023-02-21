import { ImageList, ImageListItem } from '@mui/material';
import getBase64Url from './getBase64Url';

interface PropType {
  data: string[];
  selected: string[];
  onSelect: (v: string) => void;
}

const GeneratedThumbnails = (props: PropType) => {
  const { data, selected, onSelect } = props;
  const handleClick = (v: string) => {
    onSelect(v);
  };

  return (
    <ImageList
      cols={4}
      gap={6}
      sx={{ m: 0.5 }}
    >
      {data.map((v, i) => (
        <ImageListItem
          key={i}
          sx={{
            border: selected.indexOf(v) >= 0 ? '1px solid red' : null
          }}
          onClick={() => handleClick(v)}
        >
          <img src={getBase64Url(v)}></img>
        </ImageListItem>
      ))}
    </ImageList>
  );
};

export default GeneratedThumbnails;
