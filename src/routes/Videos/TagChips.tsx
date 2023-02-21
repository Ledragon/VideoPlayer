import { Chip, Paper } from '@mui/material';
import { Tag } from '../../models/Tag';
import TagChip from './TagChip';

const TagChips = ({ tags }: { tags: Tag[] }) => {
  return (
    <Paper sx={{ p: 1, m: 1, flexGrow: 1 }}>
      {tags?.map((d) => (
        <TagChip key={d.id} tag={d}></TagChip>
      ))}
    </Paper>
  );
};

export default TagChips;
