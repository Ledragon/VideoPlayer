import { Chip, IconButton, SxProps } from '@mui/material';
import { DataGrid, GridColDef, GridRenderCellParams } from '@mui/x-data-grid';
import ImageSearchIcon from '@mui/icons-material/ImageSearch';
import VideoSettingsIcon from '@mui/icons-material/VideoSettings';
import { useState } from 'react';
import { Video } from '../../models/Video';
import getBase64Url from './getBase64Url';
import ThumbnailDialog from './Thumbnails';
import { Thumbnail } from '../../models/Thumbnail';

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
    field: 'generateThumbnails',
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
  },
  {
    field: 'details',
    headerName: '',
    width: 50,
    renderCell: (params: GridRenderCellParams<() => void>) => {
      return (
        <IconButton
          onClick={() => {
            if (params.value) {
              params.value();
            }
          }}
        >
          <VideoSettingsIcon />
        </IconButton>
      );
    }
  }
];

const VideosDataGrid = (props: {
  videos: Video[];
  onClick?: (v: Video) => void;
  thumbnails: Thumbnail[];
  sx?: SxProps;
  onSave: (thumbnails: Thumbnail[]) => void;
}) => {
  const { videos, sx, onClick, thumbnails, onSave } = props;
  const [selectedVideo, setSelectedVideo] = useState<Video | null>(null);
  const [selectedRows, setSelectedRows] = useState<any[] | undefined>(
    undefined
  );
  const [open, setOpen] = useState<boolean>(false);
  const rows = videos.map((v, i) => {
    const videoThumbs = thumbnails.filter((t) => t.videoId === v.id);
    return Object.assign({}, v, {
      id: v.fileName,
      hasTags: !!v.tags?.length,
      serializedImage: videoThumbs.length ? videoThumbs[0].image : null,
      thumbnails: videoThumbs,
      generateThumbnails: () => {
        setSelectedVideo(v);
        setOpen(true);
      },
      details: () => {
        if (onClick) {
          onClick(v);
        }
      }
    });
  });

  const handleSave = (data: Thumbnail[]) => { 
    onSave(data);
  }
  return (
    <>
      <DataGrid
        columns={columns}
        rows={rows}
        rowHeight={rowHeight}
        checkboxSelection
        onSelectionModelChange={(newSelectionModel) => {
          setSelectedRows(newSelectionModel);
        }}
        selectionModel={selectedRows}
        disableSelectionOnClick
      ></DataGrid>
      <ThumbnailDialog
        video={selectedVideo}
        onCancel={() => setOpen(false)}
        onSave={handleSave}
        open={open}
      ></ThumbnailDialog>
    </>
  );
};

export default VideosDataGrid;
