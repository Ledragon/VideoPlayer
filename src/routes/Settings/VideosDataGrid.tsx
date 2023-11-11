import { Box, Chip } from '@mui/material';
import { DataGrid, GridColDef, GridRenderCellParams, GridRowSelectionModel } from '@mui/x-data-grid';
import { useState } from 'react';

import useGetAllVideoInfo from '../../redux/hooks/useGetAllVideoInfo';

interface PropType {}

const columns: GridColDef[] = [
  // //   {
  // //     field: 'serializedImage',
  // //     headerName: 'Preview',
  // //     width: (16 / 9) * rowHeight,
  // //     align: 'center',
  // //     renderCell: (params: GridRenderCellParams<string>) => {
  // //       if (!params.value) {
  // //         return <></>;
  // //       }
  // //       return <img src={getBase64Url(params.value)} height={rowHeight} />;
  // //     }
  // //   },
  { field: 'title', headerName: 'Title', flex: 8 },
  // { field: 'fileName', headerName: 'File', flex: 4 },
  { field: 'length', headerName: 'Length', flex: 1 },
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
    renderCell: (params: GridRenderCellParams<{ value: string }[]>) => {
      if (!params.value?.length) {
        return <></>;
      }
      return (
        <>
          {params.value.map((d) => (
            <Chip key={d.value} label={d.value} variant="outlined"></Chip>
          ))}
        </>
      );
    }
  }
  //   {
  //     field: 'generateThumbnails',
  //     headerName: '',
  //     width: 40,
  //     renderCell: (params: GridRenderCellParams<() => void>) => {
  //       return (
  //         <IconButton
  //           onClick={() => {
  //             if (params.value) {
  //               params.value();
  //             }
  //           }}>
  //           <ImageSearchIcon />
  //         </IconButton>
  //       );
  //     }
  //   },
  //   {
  //     field: 'details',
  //     headerName: '',
  //     width: 50,
  //     renderCell: (params: GridRenderCellParams<() => void>) => {
  //       return (
  //         <IconButton
  //           onClick={() => {
  //             if (params.value) {
  //               params.value();
  //             }
  //           }}>
  //           <VideoSettingsIcon />
  //         </IconButton>
  //       );
  //     }
  //   }
];

const VideosDataGrid = (props: PropType) => {
  const videoInfos = useGetAllVideoInfo();
  const [rowSelectionModel, setRowSelectionModel] = useState<GridRowSelectionModel>([]);
  const rows = videoInfos?.map((v) => Object.assign({}, v.video, { tags: v.tags }));
  return (
    <DataGrid
      checkboxSelection
      columns={columns}
      rows={rows}
      onRowSelectionModelChange={(newRowSelectionModel) => {
        setRowSelectionModel(newRowSelectionModel);
      }}
      rowSelectionModel={rowSelectionModel}
    />
  );
};

export default VideosDataGrid;
