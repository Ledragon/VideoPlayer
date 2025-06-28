import { ActionIcon, Badge, Flex, Group, MultiSelect, Paper, Stack, Switch, Text, TextInput, Tooltip } from '@mantine/core';
import { IconClock, IconPlaylistAdd, IconSearch, IconSortAscendingLetters } from '@tabler/icons-react';
import { useDispatch } from 'react-redux';

import { Tag } from '../../models/Tag';
import { TagVideo } from '../../models/TagVideo';
import { Video } from '../../models/Video';
import { addVideos } from '../../redux/playlist.slice';

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

  const dispatch = useDispatch();

  const handleAddAllToPlaylist = () => {
    if (displayedVideos?.length) {
      dispatch(addVideos(displayedVideos.map((d) => d.id)));
    }
  };

  // Prepare tags for MultiSelect
  const tagOptions = tags
    .filter((t) => !tagVideos || tagVideos.filter((d) => d.tagId === t.id).length > 0)
    .map((tag) => ({
      value: tag.id.toString(),
      label: `${tag.value} (${(tagVideos || []).filter((d) => d.tagId === tag.id).length})`
    }));

  const selectedTagValues = selectedTags.map((tag) => tag.id.toString());

  const handleTagsSelectChange = (values: string[]) => {
    const selectedTagObjects = tags.filter((tag) => values.includes(tag.id.toString()));
    onTagsChange(null, selectedTagObjects);
  };

  return (
    <Paper shadow="sm" p="md" mb="md" radius="md">
      <Stack gap="md">
        {/* Top row: Search and Tags */}
        <Flex gap="md" wrap="wrap" align="flex-end">
          <TextInput
            leftSection={<IconSearch size={16} />}
            placeholder="Search videos by title..."
            onChange={onTitleSearchChange}
            style={{ flexGrow: 1, minWidth: '200px' }}
            size="sm"
          />

          <MultiSelect
            placeholder="Filter by tags..."
            data={tagOptions}
            value={selectedTagValues}
            onChange={handleTagsSelectChange}
            searchable
            clearable
            style={{ flexGrow: 2, minWidth: '300px' }}
            size="sm"
            maxDropdownHeight={200}
          />
        </Flex>

        {/* Bottom row: Actions and Controls */}
        <Flex justify="space-between" align="center" wrap="wrap" gap="md">
          <Group gap="xs">
            <Text size="sm" c="dimmed">
              {displayedVideos.length} video{displayedVideos.length !== 1 ? 's' : ''}
            </Text>
            {selectedTags.length > 0 && (
              <Badge variant="light" size="sm">
                {selectedTags.length} tag{selectedTags.length !== 1 ? 's' : ''} selected
              </Badge>
            )}
          </Group>

          <Group gap="xs">
            <Text size="sm" c="dimmed">
              Sort:
            </Text>
            <Tooltip label="Sort by title">
              <ActionIcon variant="subtle" onClick={onAlphaSort}>
                <IconSortAscendingLetters size={18} />
              </ActionIcon>
            </Tooltip>

            <Tooltip label="Sort by date">
              <ActionIcon variant="subtle" onClick={onDateSort}>
                <IconClock size={18} />
              </ActionIcon>
            </Tooltip>

            <Tooltip label="Add all to playlist">
              <ActionIcon variant="subtle" onClick={handleAddAllToPlaylist} disabled={!displayedVideos?.length}>
                <IconPlaylistAdd size={18} />
              </ActionIcon>
            </Tooltip>

            <Switch label="Contact sheets" size="sm" checked={showContactSheets} onChange={(e) => onShowContactSheetsChange(e.currentTarget.checked)} />
          </Group>
        </Flex>
      </Stack>
    </Paper>
  );
};

export default VideosFilters;
