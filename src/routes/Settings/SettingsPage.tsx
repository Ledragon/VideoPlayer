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
import NewDirectoryDialog from './NewDirectoryDialog';

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

  const [open, setOpen] = useState<boolean>(false);

  const handleAddDirectoryClick = () => {
    setOpen(true);
  };

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
      <NewDirectoryDialog
        open={open}
        setOpen={setOpen}
      />
    </>
  );
};

export default SettingsPage;
