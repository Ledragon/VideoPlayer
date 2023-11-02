import { Box, Chip, Rating, Stack, Typography } from '@mui/material';
import { Video } from '../../models/Video';
import getBase64Url from './getBase64Url';
import {
  useGetContactSheetForVideoQuery,
  useGetThumbnailsForVideoQuery
} from '../../redux';
import ThumbsImageList from './ThumbsImageList';
import { Tag } from '../../models/Tag';

interface PropType {
  tags?: (Tag | undefined)[];
  video: Video;
}

const VideoInfos = (props: PropType) => {
  const { tags, video } = props;
  const { data: contactSheet, isLoading } = useGetContactSheetForVideoQuery(
    video.id
  );
  const { data: thumbnails } = useGetThumbnailsForVideoQuery(video.id);
  return (
    <Box
      sx={{ flexGrow: 1 }}
      id="video-infos"
    >
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          flexGrow: 1,
          height: '100%',
          overflow: 'auto'
        }}
      >
        <Typography>{video.title}</Typography>
        <Rating
          value={video.rating}
          readOnly
        />
        <Box
          sx={{
            flexGrow: 1,
            flexShrink: 0,
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center'
          }}
        >
          {contactSheet && (
            <img
              src={getBase64Url(contactSheet.image)}
              style={{}}
              width="100%"
            ></img>
          )}
        </Box>
        <ThumbsImageList thumbs={thumbnails || []} />
        <Box>
          {tags?.map((tag) => (
            <Chip
              label={tag?.value}
              variant={'outlined'}
              sx={{ m: 1 }}
            />
          ))}
        </Box>
        <Stack direction="row">
          <Typography sx={{ m: 1 }}>{video.fileName}</Typography>
          <Typography sx={{ m: 1 }}>{video.length}</Typography>
        </Stack>
      </Box>
    </Box>
  );
};

export default VideoInfos;
