import { alpha, Box, LinearProgress, Paper } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';

import { MediaType } from '../../models/Directory';
import {
  useCleanVideosMutation,
  useGetDirectoriesQuery,
  useLoadVideosMutation
} from '../../redux/settings.api';
import SettingsButtons from './SettingsButtons';
import { useState } from 'react';
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
  const [loadVideos, loadVideosStatus] = useLoadVideosMutation({});
  const [cleanVideos, cleanVideosStatus] = useCleanVideosMutation();

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
          <SettingsButtons
            onAddDirectory={handleAddDirectoryClick}
            onCleanVideos={handleCleanVideos}
            onLoadVideos={handleLoadVideos}
          />
          {cleanVideosStatus.isLoading || loadVideosStatus.isLoading ? (
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
