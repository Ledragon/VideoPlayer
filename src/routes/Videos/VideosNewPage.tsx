import {
  Box,
  Card,
  CardHeader,
  CardMedia,
  Chip,
  Paper,
  Stack,
  Typography
} from '@mui/material';
import { useState } from 'react';
import { Video } from '../../models/Video';
import {
  useGetVideosQuery,
  useGetThumbnailsQuery,
  useGetTagsQuery,
  useGetTagVideosQuery
} from '../../redux';
import getBase64Url from './getBase64Url';
import VideosImageList from './VideosImageList';

interface PropType {}
const VideosNewPage = (props: PropType) => {
  const [video, setSelectedVideo] = useState<Video | null>(null);
  const { data: videos, isLoading } = useGetVideosQuery();
  const { data: thumbnails, isLoading: isLoadingThumbnails } =
    useGetThumbnailsQuery();
  const { data: tags, isLoading: isLoadingTags } = useGetTagsQuery();
  const { data: tagVideos, isLoading: isLoadingTagVideos } =
    useGetTagVideosQuery();
  const selectedThumbs =
    thumbnails?.filter((t) => t.videoId === video?.id) || [];
  return (
    <Box sx={{ display: 'flex', flexDirection: 'row', p: 1, height: '100%' }}>
      <Box
        sx={{ flexBasis: '20%', display: 'flex', flexShrink: 0 }}
        id="tags-chips"
      >
        <Paper sx={{ p: 1, m: 1, flexGrow: 1 }}>
          {tags?.map((d) => (
            <Chip
              key={d.id}
              label={d.value}
              variant="outlined"
              sx={{ m: 1 }}
            ></Chip>
          ))}
        </Paper>
      </Box>
      <Box
        sx={{
          flexBasis: '80%',
          display: 'flex',
          flexDirection: 'column',
          overflow: 'hidden'
        }}
      >
        <Box
          sx={{
            flexBasis: '60%',
            display: 'flex',
            flexGrow: 0,
            flexShrink: 0
          }}
          id="video-details"
        >
          <Paper sx={{ flexGrow: 1, p: 1, m: 1 }}>
            {!!video ? (
              <Box sx={{ display: 'flex', flexDirection: 'row' }}>
                <Box
                  sx={{
                    flexBasis: '50%',
                    display: 'flex',
                    flexDirection: 'column',
                    p: 1
                  }}
                >
                  <Box sx={{ flexbasis: '50%' }}>
                    <img
                      src={getBase64Url(selectedThumbs[0].image)}
                      style={{
                        backgroundSize: 'cover',
                        backgroundPosition: 'center',
                        backgroundRepeat: 'no-repeat',
                        objectFit: 'cover',
                        height: '180px'
                      }}
                    ></img>
                  </Box>
                  <Box sx={{ flexbasis: '50%' }}>
                    <img
                      src={getBase64Url(selectedThumbs[0].image)}
                      style={{
                        backgroundSize: 'cover',
                        backgroundPosition: 'center',
                        backgroundRepeat: 'no-repeat',
                        objectFit: 'cover',
                        flexBasis: '100%'
                      }}
                    ></img>
                  </Box>
                </Box>
                <Box
                  sx={{
                    flexBasis: '50%',
                    display: 'flex',
                    flexDirection: 'column'
                  }}
                >
                  <Typography variant="body2">{video.title}</Typography>
                  <Typography variant="body2">{video.length}</Typography>
                  <Typography variant="body2">{video.lastPlayed}</Typography>
                </Box>
              </Box>
            ) : (
              <></>
            )}
          </Paper>
        </Box>
        <Box
          id="videos-list-container"
          sx={{ flexGrow: 0, flexBasis: '30%', flexShrink: 0 }}
        >
          <Paper
            sx={{
              flexGrow: 1,
              p: 1,
              m: 1,
              display: 'flex'
            }}
          >
            {!!videos ? (
              videos.map((v) => {
                const thumbs =
                  thumbnails?.filter((t) => t.videoId == v.id) || [];
                return (
                  <Card
                    key={v.id}
                    sx={{
                      flexBasis: '250px',
                      flexGrow: 0,
                      flexShrink: 0,
                      m: 1
                    }}
                    onClick={() => setSelectedVideo(v)}
                  >
                    <CardMedia
                      image={thumbs.length ? getBase64Url(thumbs[0].image) : ''}
                      height={180}
                      component="img"
                    ></CardMedia>
                    <CardHeader
                      title={v.title}
                      subheader={v.length}
                    ></CardHeader>
                  </Card>
                );
              })
            ) : (
              <></>
            )}
            {/* <VideosImageList
              videos={videos || []}
              onClick={(v) => {
                setSelectedVideo(v);
              }}
            ></VideosImageList> */}
            {/* <Stack
              sx={{ width: '100%', overflowX: 'auto' }}
              direction="row"
              spacing={2}
            >
              {videos?.map((v) => {
                const thumbs =
                  thumbnails?.filter((t) => t.videoId == v.id) || [];
                return (
                  <Card key={v.id} sx={{ minWidth: 250 }}>
                    <CardMedia
                      image={thumbs.length ? getBase64Url(thumbs[0].image) : ''}
                      height={180}
                      component="img"
                    ></CardMedia>
                    <CardHeader title={v.title}></CardHeader>
                  </Card>
                );
              })}
            </Stack> */}
          </Paper>
        </Box>
      </Box>
    </Box>
  );
};

export default VideosNewPage;
