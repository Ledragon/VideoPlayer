import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField
} from '@mui/material';

import { MediaType } from '../../models/Directory';
import { useFormik } from 'formik';
import { useAddDirectoryMutation } from '../../redux/settings.api';

interface PropType {
  open: boolean;
  setOpen: (open: boolean) => void;
}

const NewDirectoryDialog = (props: PropType) => {
    const { open, setOpen } = props;
    const [addDirectory, addDirectoryStatus] = useAddDirectoryMutation();
    
  const formik = useFormik({
    initialValues: {
      directoryPath: '',
      directoryName: '',
      isIncludeSubdirectories: true,
      loadContent: true
    },
    onSubmit: (values) => {
      addDirectory({
        directory: Object.assign({}, values, { mediaType: MediaType.Video }),
        loadContent: values.loadContent
      });
    }
  });
    
  return (
    <Dialog
      maxWidth="lg"
      open={open}
      onClose={() => setOpen(false)}
    >
      <DialogTitle>Add new directory</DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'flex', flexDirection: 'column' }}>
          <TextField
            fullWidth={true}
            name="directoryName"
            value={formik.values.directoryName}
            error={!!formik.errors.directoryName}
            label="Name"
            size="small"
            margin="dense"
            onChange={formik.handleChange}
          />
          <TextField
            fullWidth={true}
            name="directoryPath"
            value={formik.values.directoryPath}
            error={!!formik.errors.directoryPath}
            label="Path"
            size="small"
            margin="dense"
            onChange={formik.handleChange}
          />
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={() => setOpen(false)}>Cancel</Button>
        <Button
          onClick={() => {
            formik.submitForm();
          }}
        >
          Save
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default NewDirectoryDialog;
