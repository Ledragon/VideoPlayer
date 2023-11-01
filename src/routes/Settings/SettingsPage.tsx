import { alpha, Box, LinearProgress, Paper } from '@mui/material';
import { DataGrid, GridColDef, GridSelectionModel } from '@mui/x-data-grid';

import { MediaType } from '../../models/Directory';
import {
  useCleanVideosMutation,
  useGetDirectoriesQuery,
  useLoadVideosMutation,
  useRemoveDirectoryMutation
} from '../../redux/settings.api';
import SettingsButtons from './SettingsButtons';
import { useState } from 'react';
import NewDirectoryDialog from './NewDirectoryDialog';
import { useAutoCompleteLibraryMutation } from '../../redux';

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
  const [selectedRows, setSelectedRows] = useState<number[]>([]);

  const { isFetching, data } = useGetDirectoriesQuery();
  const [loadVideos, loadVideosStatus] = useLoadVideosMutation({});
  const [cleanVideos, cleanVideosStatus] = useCleanVideosMutation();
  const [removeDirectory, removeDirectoryStatus] = useRemoveDirectoryMutation();
  const [autoComplete, autoCompleteStatus] = useAutoCompleteLibraryMutation();

  const [open, setOpen] = useState<boolean>(false);

  const handleAddDirectoryClick = () => {
    setOpen(true);
  };
  const handleLoadVideos = () => {
    loadVideos();
  };

  const handleCleanVideos = () => {
    cleanVideos();
  };

  const handleAutoComplete = () => {
    autoComplete();
  };

  const handleRemoveDirectory = () => {
    selectedRows.forEach((r) => {
      removeDirectory(r);
    });
  };

  const handleSelectChange = (selectionModel: GridSelectionModel) => {
    setSelectedRows(selectionModel.map((d) => d as number));
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
            loading={isFetching}
            onSelectionModelChange={handleSelectChange}
            hideFooter
          />
          <SettingsButtons
            onAddDirectory={handleAddDirectoryClick}
            onAutoComplete={handleAutoComplete}
            onCleanVideos={handleCleanVideos}
            onLoadVideos={handleLoadVideos}
            onRemoveDirectory={handleRemoveDirectory}
          />
          {cleanVideosStatus.isLoading ||
          loadVideosStatus.isLoading ||
          removeDirectoryStatus.isLoading ||
          autoCompleteStatus.isLoading ? (
            <LinearProgress />
          ) : null}
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
