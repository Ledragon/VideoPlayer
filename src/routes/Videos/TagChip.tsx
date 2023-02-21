import { Chip, Paper } from '@mui/material';
import { Tag } from '../../models/Tag';

interface PropType {
  tag: Tag;
  onClick?: (tag: Tag) => void;
}

const TagChip = ({ onClick, tag }: PropType) => {
  const handleClick = () => {
    if (onClick) {
      onClick(tag);
    }
  };
  return (
    <Chip
      label={tag.value}
      variant="outlined"
      sx={{ m: 1 }}
      onClick={handleClick}
    ></Chip>
  );
};

export default TagChip;
