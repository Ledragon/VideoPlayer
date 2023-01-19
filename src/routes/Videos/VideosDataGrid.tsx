import { Button, Chip, IconButton, SxProps } from '@mui/material';
import {
  DataGrid,
  GridColDef,
  GridRenderCellParams,
  GridToolbar
} from '@mui/x-data-grid';
import ImageSearchIcon from '@mui/icons-material/ImageSearch';
import { useState } from 'react';
import { Video } from '../../models/Video';
import getBase64Url from './getBase64Url';
import ThumbnailDialog from './Thumbnails';
import { apiUrl } from '../../fetchHelper';

const rowHeight = 64;

const columns: GridColDef[] = [
  {
    field: 'serializedImage',
    headerName: 'Preview',
    width: (16 / 9) * rowHeight,
    align: 'center',
    renderCell: (params: GridRenderCellParams<string>) => {
      if (!params.value) {
        return <></>;
      }
      return <img src={getBase64Url(params.value)} height={rowHeight} />;
    }
  },
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
  { field: 'category', headerName: 'Category', flex: 1 },
  {
    field: 'tags',
    headerName: 'Tags',
    flex: 2,
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
  },
  {
    field: 'thumbnails',
    headerName: '',
    width: 40,
    renderCell: (params: GridRenderCellParams<() => void>) => {
      return (
        <IconButton
          onClick={() => {
            if (params.value) {
              params.value();
            }
          }}
        >
          <ImageSearchIcon />
        </IconButton>
      );
    }
  }
];

const VideosDataGrid = (props: {
  videos: Video[];
  onClick?: (v: Video) => void;
  sx?: SxProps;
}) => {
  const { videos, sx } = props;
  const [selectedVideo, setSelectedVideo] = useState<Video | null>(null);
  const [open, setOpen] = useState<boolean>(false);
  const rows = videos.map((v, i) => {
    return Object.assign({ id: v.fileName,
      hasTags: !!v.tags?.length,
      thumbnails: () => {
        setSelectedVideo(v);
        setOpen(true);
      }
    }, v);
  });
  return (
    <>
      (
    <DataGrid
      columns={columns}
      rows={rows}
      rowHeight={rowHeight}
      checkboxSelection
      onSelectionModelChange={(newSelectionModel) =>
        console.log(newSelectionModel)
      }
      disableSelectionOnClick
    ></DataGrid>
      <ThumbnailDialog
        video={selectedVideo}
        onCancel={() => setOpen(false)}
        open={open}
      ></ThumbnailDialog>
    </>
  )
  );
};

export default VideosDataGrid;
