import { Alert, Box, Skeleton, Typography } from '@mui/material';
import { FormikProps } from 'formik';

import { Tag } from '../../models/Tag';
import { TagVideo } from '../../models/TagVideo';
import { Video } from '../../models/Video';
import { useGetContactSheetForVideoQuery } from '../../redux';
import ContactSheetForm from './ContactSheetForm';
import CreateTag from './CreateTag';
import getBase64Url from './getBase64Url';
import VideoProperties from './VideoProperties';

interface PropType {
  error: any;
  formik: FormikProps<any>;
  tags: Tag[];
  tagVideos: TagVideo[];
  video: Video;
  onSave: (video: Video) => void;
  selectedThumbnails: string[];
  setSelectedThumbnails: (newValue: string[]) => void;
}

const VideoEdtion = (props: PropType) => {
  const { tagVideos, error, formik, onSave, selectedThumbnails, setSelectedThumbnails, tags, video } = props;

  const { data: contactSheet, isLoading } = useGetContactSheetForVideoQuery(video.id);

  return (
    <>
      <Box sx={{ display: 'flex', flexDirection: 'row', flexGrow: 1 }}>
        <Box
          className="properties-container"
          sx={{
            flexBasis: '100%',
            display: 'flex',
            flexDirection: 'column',
            flexGrow: 0,
            flexShrink: 0,
            overflow: 'hidden'
          }}>
          <VideoProperties formik={formik} tags={tags} tagVideos={tagVideos} video={video} onSubmit={onSave} />
          <Box>
            <CreateTag />
          </Box>
          {isLoading ? (
            <Skeleton
              variant="rounded"
              sx={{
                flexGrow: 1,
                width: '100%'
              }}
            />
          ) : null}
          {contactSheet && !isLoading ? (
            <Box
              sx={{
                flexGrow: 1,
                overflow: 'auto',
                flexShrink: 1,
                flexBasis: '100%'
              }}>
              <Box
                sx={{
                  flexGrow: 1,
                  backgroundImage: `url('${getBase64Url(contactSheet.image)}')`,
                  height: '100%',
                  backgroundRepeat: 'no-repeat',
                  backgroundSize: 'contain',
                  backgroundPosition: 'center'
                }}
              />
            </Box>
          ) : (
            <></>
          )}
          {!contactSheet ? <Typography>No contact sheet</Typography> : <></>}
          <Box sx={{ display: 'flex', flexGrow: 0, flexShrink: 0 }}>
            <ContactSheetForm video={video} />
          </Box>
        </Box>
      </Box>

      <Box>{error ? <Alert severity="error">An error occured. Check console for more information.</Alert> : null}</Box>
      <Box id="info-bar" sx={{ display: 'flex' }}>
        <Typography variant="caption">{video.fileName}</Typography>
      </Box>
    </>
  );
};

export default VideoEdtion;
