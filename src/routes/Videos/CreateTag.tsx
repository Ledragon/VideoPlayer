import { Alert, Box, Button, Snackbar, TextField } from '@mui/material';
import { useState } from 'react';
import { useSelector } from 'react-redux';

import { getTagList, useAddTagMutation } from '../../redux';
import { RootState } from '../../redux/configureStore';

interface PropType {}

const CreateTag = (props: PropType) => {
  const [newTag, setNewTag] = useState<{
    value: string;
    isValid: boolean;
    errorMessage?: string;
  }>({ value: '', isValid: false, errorMessage: 'A value is required' });
  const [snackOpen, setSnackOpen] = useState(false);
  const [snackMessage, setSnackMessage] = useState<string>();

  const tags = useSelector((state: RootState) => getTagList(state));
  const [addTag, addTagStatus] = useAddTagMutation();

  const handleTagChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    const isValid = !!value && !tags.filter((t) => t.value.localeCompare(value) === 0).length;
    setNewTag({ value, isValid });
  };

  const handleAddTag = () => {
    addTag({ id: 0, value: newTag.value })
      .unwrap()
      .then(() => {
        setSnackOpen(true);
        setNewTag({ value: '', isValid: true });
        setSnackMessage('Tag added');
      })
      .catch((e) => {
        setSnackOpen(true);
        setSnackMessage('Error adding tag: ' + e.message);
      });
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
      <Button variant="outlined" onClick={handleAddTag}>
        Add
      </Button>
      <Snackbar
        open={snackOpen}
        autoHideDuration={3000}
        onClose={() => {
          setSnackOpen(false);
        }}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}>
        <Alert severity={addTagStatus.isError ? 'error' : 'success'} variant="outlined">
          {snackMessage}
        </Alert>
      </Snackbar>
    </Box>
  );
};

export default CreateTag;
