import { LinearProgress } from '@mui/material';
import { DataGrid, GridColDef, GridRowSelectionModel } from '@mui/x-data-grid';
import { useState } from 'react';

import { MediaType } from '../../models/Directory';
import { useAutoCompleteLibraryMutation } from '../../redux';
import { useCleanVideosMutation, useGetDirectoriesQuery, useLoadVideosMutation, useRemoveDirectoryMutation } from '../../redux/settings.api';
import NewDirectoryDialog from './NewDirectoryDialog';
import SettingsButtons from './SettingsButtons';

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
const DirectoriesSettings = (props: PropType) => {
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

  const handleSelectChange = (selectionModel: GridRowSelectionModel) => {
    setSelectedRows(selectionModel.map((d) => d as number));
  };
  return (
    <>
      <DataGrid columns={cols} rows={data || []} loading={isFetching} onRowSelectionModelChange={handleSelectChange} hideFooter />
      <SettingsButtons
        onAddDirectory={handleAddDirectoryClick}
        onAutoComplete={handleAutoComplete}
        onCleanVideos={handleCleanVideos}
        onLoadVideos={handleLoadVideos}
        onRemoveDirectory={handleRemoveDirectory}
      />
      {cleanVideosStatus.isLoading || loadVideosStatus.isLoading || removeDirectoryStatus.isLoading || autoCompleteStatus.isLoading ? <LinearProgress /> : null}
      <NewDirectoryDialog open={open} setOpen={setOpen} />
    </>
  );
};

export default DirectoriesSettings;
