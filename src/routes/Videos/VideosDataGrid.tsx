import { Box, Button, Chip } from '@mui/material';
import { DataGrid, GridColDef, GridRenderCellParams } from '@mui/x-data-grid';
import { useState } from 'react';
import { Video } from '../../models/Video';
import getBase64Url from './getBase64Url';
import ThumbnailDialog from './Thumbnails';

const columns: GridColDef[] = [
  {
    field: 'serializedImage',
    headerName: 'Preview',
    width: (16 / 9) * 128,
    align: 'center',
    renderCell: (params: GridRenderCellParams<string>) => {
      if (!params.value) {
        return <></>;
      }
      return <img src={getBase64Url(params.value)} height={128} />;
    }
  },
  { field: 'title', headerName: 'Title', flex: 8 },
  { field: 'fileName', headerName: 'File', flex: 4 },
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
    field: 'action',
    width: 40,
    renderCell: (params: GridRenderCellParams<() => void>) => {
      return (
        <Button
          onClick={() => {
            if (params.value) {
              params.value();
            }
          }}
        >
          Generate
        </Button>
      );
    }
  }
];

const VideosDataGrid = (props: {
  videos: Video[];
  onClick?: (v: Video) => void;
}) => {
  const { videos } = props;
  const [selectedVideo, setSelectedVideo] = useState<Video | null>(null);
  const [open, setOpen] = useState<boolean>(false);
  const rows = videos.map((v, i) => {
    return Object.assign({ id: i }, v, {
      hasTags: !!v.tags?.length,
      action: () => {
        setSelectedVideo(v);
        setOpen(true);
      }
    });
  });
  return (
    <>
      <DataGrid columns={columns} rows={rows} rowHeight={128}></DataGrid>
      <ThumbnailDialog
        video={selectedVideo}
        onCancel={() => setOpen(false)}
        open={open}
      ></ThumbnailDialog>
    </>
  );
};

export default VideosDataGrid;
