import { Paper, alpha } from '@mui/material';
import { Tag } from '../../models/Tag';
import TagChip from './TagChip';

const TagChips = ({
  selectedTags,
  tags,
  onTagClick
}: {
  selectedTags: Tag[];
  tags: Tag[];
  onTagClick?: (tag: Tag) => void;
}) => {
  return (
    <Paper
      sx={{
        p: 1,
        m: 1,
        flexGrow: 1,
        backgroundColor: (theme) => alpha(theme.palette.background.paper, 0.6)
      }}
    >
      {tags?.map((d) => (
        <TagChip
          key={d.id}
          selected={selectedTags.indexOf(d) >= 0}
          tag={d}
          onClick={onTagClick}
        ></TagChip>
      ))}
    </Paper>
  );
};

export default TagChips;
