import NavigateBeforeIcon from '@mui/icons-material/NavigateBefore';
import NavigateAfterIcon from '@mui/icons-material/NavigateNext';
import { Alert, Box, Button, CircularProgress, Dialog, DialogActions, DialogContent, DialogTitle, IconButton, Skeleton, Typography } from '@mui/material';
import { FormikConfig, FormikProps, useFormik } from 'formik';
import { useMemo, useState } from 'react';

import { Tag } from '../../models/Tag';
import { TagVideo } from '../../models/TagVideo';
import { Video } from '../../models/Video';
import { useGenerateThumbnailsMutation, useGetContactSheetForVideoQuery, useGetThumbnailsForVideoQuery } from '../../redux';
import AddToPlaylist from './AddToPlaylist';
import ContactSheetForm from './ContactSheetForm';
import CreateTag from './CreateTag';
import GeneratedThumbnails from './GeneratedThumbnails';
import getBase64Url from './getBase64Url';
import ThumbnailsGenerator from './ThumbnailsGenerator';
import ThumbsImageList from './ThumbsImageList';
import VideoInfos from './VideoInfos';
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
  const [_, status] = useGenerateThumbnailsMutation({
    fixedCacheKey: video.fileName
  });
  const { data: thumbnails } = useGetThumbnailsForVideoQuery(video.id);

  const handleSelectThumdbnail = (v: string) => {
    const index = selectedThumbnails.indexOf(v);
    if (index >= 0) {
      selectedThumbnails.splice(index, 1);
    } else {
      selectedThumbnails.push(v);
    }
    setSelectedThumbnails(selectedThumbnails.map((d) => d));
  };

  return (
    <>
      <Box sx={{ display: 'flex', flexDirection: 'row', flexGrow: 1 }}>
        <Box
          className='properties-container'
          sx={{
            flexBasis: '50%',
            display: 'flex',
            flexDirection: 'column',
            flexGrow: 0,
            flexShrink: 0,
            overflow: 'hidden'
          }}>
          <VideoProperties formik={formik} tags={tags} tagVideos={tagVideos} video={video} onSubmit={onSave} />
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
                <Box sx={{flexGrow:1, backgroundImage:`url('${getBase64Url(contactSheet.image)}')`, height :'100%', backgroundRepeat: 'no-repeat', backgroundSize: 'contain', backgroundPosition: 'center' }}/>
              {/* <img src={getBase64Url(contactSheet.image)} style={{}}></img> */}
            </Box>
          ) : (
            <></>
          )}
          <Box sx={{ display: 'flex', flexGrow:  0, flexShrink: 0}}>
            <ContactSheetForm video={video} />
          </Box>
        </Box>
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            flexBasis: '50%',
            flexGrow: 0,
            flexShrink: 0
          }}>
          <Box>
            <CreateTag />
          </Box>
          <Box sx={{ flexBasis: '50%' }}>
            <ThumbsImageList thumbs={thumbnails || []} />
          </Box>
          <Box sx={{ flexBasis: '50%' }}>
            <ThumbnailsGenerator video={video} />
            {status.isLoading ? (
              <CircularProgress />
            ) : (
              <GeneratedThumbnails data={status?.data || []} selected={selectedThumbnails} onSelect={handleSelectThumdbnail}></GeneratedThumbnails>
            )}
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
