import { Autocomplete, Box, Button, Chip, Dialog, DialogActions, DialogContent, TextField } from '@mui/material';
import {
  DataGrid,
  GridColDef,
  GridRenderCellParams,
  GridRowSelectionModel,
  GridToolbar,
  GridToolbarContainer,
  GridToolbarDensitySelector
} from '@mui/x-data-grid';
import { useState } from 'react';
import { useSelector } from 'react-redux';

import { Tag } from '../../models/Tag';
import { TagVideo } from '../../models/TagVideo';
import { getTagList, useAddTagVideosMutation } from '../../redux';
import useGetAllVideoInfo from '../../redux/hooks/useGetAllVideoInfo';

interface PropType {}

const columns: GridColDef[] = [
  { field: 'title', headerName: 'Title', flex: 6 },
  { field: 'length', headerName: 'Length', flex: 1 },
  { field: 'directoryPath', headerName: 'Directory', flex: 3 },
  {
    field: 'dateAdded',
    headerName: 'Date added',
    flex: 2,
    valueFormatter: (params) => {
      if (params.value == null) {
        return '';
      }
      return new Date(params.value).toLocaleString();
    }
  },
  {
    field: 'tags',
    headerName: 'Tags',
    flex: 4,
    type: 'string',
    renderCell: (params: GridRenderCellParams<{ value: string }[]>) => {
      if (!params.value?.length) {
        return <></>;
      }
      return (
        <>
          {params.value.map((d: Tag) => (
            <Chip key={d.value} label={d.value} variant="outlined"></Chip>
          ))}
        </>
      );
    },
    filterable: false,
    sortComparator: (t1: Tag[], t2: Tag[]) => {
      if (!t1.length) {
        return -1;
      } else if (!t2.length) {
        return 1;
      } else {
        return t1[0].value > t2[0].value ? 1 : -1;
      }
    }
  }
];

const Toolbar = ({ rows }: { rows: GridRowSelectionModel }) => {
  const [open, setOpen] = useState(false);
  const [updatedTags, setUpdatedTags] = useState<Tag[]>([]);

  const [addTags] = useAddTagVideosMutation();

  const tags = useSelector(getTagList);

  const handleAddTags = () => {
    setOpen(true);
  };

  const handleCancel = () => {
    setUpdatedTags([]);
    setOpen(false);
  };

  const handleSave = async () => {
    const toAdd = rows.flatMap((r) => updatedTags.map((t) => ({ videoId: +r, tagId: t.id })));
    await addTags(toAdd).unwrap();
    setUpdatedTags([]);
    setOpen(false);
  };

  const handleTagsChange = (_event: React.ChangeEvent<any>, value: Tag[]) => {
    setUpdatedTags(value);
    console.log(value);
  };

  return (
    <>
      <GridToolbarContainer>
        <GridToolbarDensitySelector />
        <Button disabled={!rows?.length} onClick={handleAddTags}>
          Add tags
        </Button>
      </GridToolbarContainer>
      <Dialog open={open} onClose={() => setOpen(false)} maxWidth="md">
        <DialogContent>
          <Autocomplete
            multiple
            id="tags"
            options={tags || []}
            getOptionLabel={(o) => o?.value || ''}
            onChange={handleTagsChange}
            value={updatedTags}
            renderInput={(params) => <TextField {...params} variant="standard" label="Tags" placeholder="Tags" margin="dense" />}></Autocomplete>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCancel}>Cancel</Button>
          <Button variant="contained" color="primary" onClick={handleSave}>
            Save
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

const VideosDataGrid = (props: PropType) => {
  const videoInfos = useGetAllVideoInfo();
  const [rowSelectionModel, setRowSelectionModel] = useState<GridRowSelectionModel>([]);
  const rows = videoInfos?.map((v) => Object.assign({}, v.video, { tags: v.tags }));
  return (
    <DataGrid
      checkboxSelection
      columns={columns}
      density="compact"
      rows={rows}
      slots={{
        toolbar: Toolbar
      }}
      slotProps={{ toolbar: { rows: rowSelectionModel } }}
      onRowSelectionModelChange={(newRowSelectionModel) => {
        setRowSelectionModel(newRowSelectionModel);
      }}
      rowSelectionModel={rowSelectionModel}
    />
  );
};

export default VideosDataGrid;
