import ImageSearchIcon from '@mui/icons-material/ImageSearch';
import VideoSettingsIcon from '@mui/icons-material/VideoSettings';
import { Chip, IconButton, SxProps } from '@mui/material';
import { DataGrid, GridColDef, GridRenderCellParams } from '@mui/x-data-grid';
import { useState } from 'react';

import ThumbnailDialog from './Thumbnails';
import getBase64Url from './getBase64Url';
import { Tag } from '../../models/Tag';
import { Thumbnail } from '../../models/Thumbnail';
import { Video } from '../../models/Video';
import { TagVideo } from '../../models/TagVideo';

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
  tags?: Tag[];
  tagVideos?: TagVideo[];
  thumbnails: Thumbnail[];
  sx?: SxProps;
  onSave: (thumbnails: Thumbnail[]) => void;
}) => {
  const { videos, onClick, tags, tagVideos, thumbnails, onSave } = props;
  const [selectedVideo, setSelectedVideo] = useState<
    | {
        video: Video;
        thumbnails: Thumbnail[];
      }
    | undefined
  >(undefined);
  const [selectedRows, setSelectedRows] = useState<any[] | undefined>(
    undefined
  );
  const [open, setOpen] = useState<boolean>(false);
  const rows = videos.map((v, i) => {
    const videoThumbs = thumbnails.filter((t) => t.videoId === v.id);
    const currentTags =
      tagVideos && tags
        ? tagVideos
            .filter((tv) => tv.videoId == v.id)
            .map((tv) => tags.find((t) => t.id === tv.tagId))
        : [];

    return Object.assign({}, v, {
      id: v.id,
      hasTags: !!v.tags?.length,
      serializedImage: videoThumbs.length ? videoThumbs[0].image : null,
      thumbnails: videoThumbs,
      tags: currentTags,
      generateThumbnails: () => {
        setSelectedVideo({ video: v, thumbnails: videoThumbs });
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
  };
  return (
    <>
      <DataGrid
        columns={columns}
        rows={rows}
        rowHeight={rowHeight}
        checkboxSelection
        onSelectionModelChange={(newSelectionModel) => {
          setSelectedRows(newSelectionModel);
          console.log(newSelectionModel);
        }}
        selectionModel={selectedRows}
        disableSelectionOnClick={true}
      ></DataGrid>
      <ThumbnailDialog
        tuple={selectedVideo}
        onCancel={() => setOpen(false)}
        onSave={handleSave}
        open={open}
      ></ThumbnailDialog>
    </>
  );
};

export default VideosDataGrid;
