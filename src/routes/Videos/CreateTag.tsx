import { Box, Button, TextField } from '@mui/material';
import { useState } from 'react';
import { getTagList, useAddTagMutation } from '../../redux';
import { RootState } from '../../redux/configureStore';
import { useSelector } from 'react-redux';

interface PropType {}

const CreateTag = (props: PropType) => {
  const [newTag, setNewTag] = useState<{
    value: string;
    isValid: boolean;
    errorMessage?: string;
  }>({ value: '', isValid: false, errorMessage: 'A value is required' });

  const tags = useSelector((state: RootState) => getTagList(state));
  const [addTag, addTagStatus] = useAddTagMutation();

  const handleTagChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    const isValid =
      !!value && !tags.filter((t) => t.value.localeCompare(value) === 0).length;
    setNewTag({ value, isValid });
  };

  const handleAddTag = () => {
    addTag({ id: 0, value: newTag.value });
  };
  
  return (
    <Box sx={{ display: 'flex', alignItems: 'center' }}>
      <TextField
        fullWidth
        margin="dense"
        id="new-tag"
        name="tag"
        onChange={handleTagChange}
        label="New tag"
        value={newTag.value}
        error={!newTag.isValid}
        variant="standard"
      />
      <Button
        variant="outlined"
        onClick={handleAddTag}
      >
        Add
      </Button>
    </Box>
  );
};

export default CreateTag;
