import { Chip, Paper } from '@mui/material';
import { Tag } from '../../models/Tag';

interface PropType {
  tag: Tag;
  selected?: boolean;
  onClick?: (tag: Tag) => void;
}

const TagChip = ({ onClick, selected, tag }: PropType) => {
  const handleClick = () => {
    if (onClick) {
      onClick(tag);
    }
  };
  return (
    <Chip
      label={tag.value}
      variant={selected ? 'filled' : 'outlined'}
      sx={{ m: 1 }}
      onClick={handleClick}
    />
  );
};

export default TagChip;
