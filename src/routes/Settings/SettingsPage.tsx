import {
  alpha,
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Paper,
  TextField
} from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';

import { MediaType } from '../../models/Directory';
import {
  useAddDirectoryMutation,
  useGetDirectoriesQuery
} from '../../redux/settings.api';
import SettingsButtons from './SettingsButtons';
import { useState } from 'react';
import { useFormik } from 'formik';

interface PropType {}
const cols: GridColDef[] = [
  {
    field: 'directoryPath',
    headerName: 'Directory',
    flex: 1
  },
  {
    headerName: 'Include sub directories',
    field: 'isIncludeSubdirectories'
  },
  {
    headerName: 'Type',
    field: 'mediaType',
    valueFormatter: (d) => MediaType[d.value]
  }
];

const SettingsPage = (props: PropType) => {
  const { isFetching, data } = useGetDirectoriesQuery();
  const [addDirectory, addDirectoryStatus] = useAddDirectoryMutation();

  const [open, setOpen] = useState<boolean>(false);

  const handleClose = () => {
    setOpen(false);
  };

  const handleAddDirectoryClick = () => {
    setOpen(true);
  };

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
    <>
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'row',
          flexGrow: 1,
          overflow: 'hidden',
          p: 1
        }}
      >
        <Paper
          sx={{
            flexGrow: 1,
            m: 1,
            backgroundColor: (theme) =>
              alpha(theme.palette.background.paper, 0.7),
            position: 'relative'
          }}
        >
          <DataGrid
            columns={cols}
            rows={data || []}
            hideFooter
            loading={isFetching}
          />
          <SettingsButtons onAddDirectory={handleAddDirectoryClick} />
        </Paper>
      </Box>
      <Dialog
        maxWidth="lg"
        open={open}
        onClose={handleClose}
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
    </>
  );
};

export default SettingsPage;
