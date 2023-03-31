import { alpha, Box, Paper } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { MediaType } from '../../models/Directory';
import { useGetDirectoriesQuery } from '../../redux/settings.api';

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
  return (
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
          backgroundColor: (theme) => alpha(theme.palette.background.paper, 0.7)
        }}
      >
        <DataGrid
          columns={cols}
          rows={data || []}
          hideFooter
          loading={isFetching}
        />
      </Paper>
    </Box>
  );
};

export default SettingsPage;
