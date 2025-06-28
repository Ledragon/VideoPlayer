import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';
import SortByAlphaOutlinedIcon from '@mui/icons-material/SortByAlphaOutlined';
import { Autocomplete, Box, FormControlLabel, IconButton, Paper, Switch, TextField } from '@mui/material';

import { Tag } from '../../models/Tag';
import { TagVideo } from '../../models/TagVideo';
import { Video } from '../../models/Video';
import AddAllToPlaylist from './AddAllToPlaylist';

interface PropType {
  displayedVideos: Video[];
  onAlphaSort: () => void;
  onDateSort: () => void;
  onShowContactSheetsChange: (checked: boolean) => void;
  onTagsChange: (event: any, values: Tag[]) => void;
  onTitleSearchChange: (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => void;
  selectedTags: Tag[];
  showContactSheets: boolean;
  tagVideos: TagVideo[];
  tags: Tag[];
}

const VideosFilters = (props: PropType) => {
  const {
    tags,
    tagVideos,
    selectedTags,
    displayedVideos,
    showContactSheets,
    onTagsChange,
    onTitleSearchChange,
    onAlphaSort,
    onDateSort,
    onShowContactSheetsChange
  } = props;

  return (
    <Paper id="tags-chips" sx={{ flex: '1 0 auto', p: 0.5, display: 'flex' }}>
      <Autocomplete
        sx={{ flexGrow: 1, m: 0.5, flexBasis: '60%' }}
        multiple
        id="tags"
        options={tags.filter((t) => !tagVideos || tagVideos.filter((d) => d.tagId === t.id).length > 0) || []}
        getOptionLabel={(o) => (o ? `${o.value} (${(tagVideos || []).filter((d) => d.tagId === o.id).length})` : '')}
        onChange={onTagsChange}
        value={selectedTags}
        renderInput={(params) => <TextField {...params} variant="standard" label="Tags" placeholder="Tags" margin="dense" sx={{ m: 0.5 }} />}
      />
      <TextField sx={{ flexGrow: 1, m: 0.5, flexBasis: '20%' }} onChange={onTitleSearchChange} placeholder="Title" />
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center'
        }}>
        <IconButton size="small" onClick={onAlphaSort} title="Sort by title">
          <SortByAlphaOutlinedIcon fontSize="small" />
        </IconButton>
        <IconButton size="small" onClick={onDateSort} title="Sort by date">
          <AccessTimeOutlinedIcon fontSize="small" />
        </IconButton>
        <AddAllToPlaylist videos={displayedVideos} />
        <FormControlLabel
          control={
            <Switch
              size="small"
              checked={showContactSheets}
              onChange={(e, checked) => {
                onShowContactSheetsChange(checked);
              }}
            />
          }
          label="Contact sheets"
          sx={{ mx: 1 }}
        />
      </Box>
    </Paper>
  );
};

export default VideosFilters;
