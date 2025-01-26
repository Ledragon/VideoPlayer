import { Box, CircularProgress } from '@mui/material';

import { Video } from '../../models/Video';
import { useGenerateThumbnailsMutation, useGetThumbnailsForVideoQuery } from '../../redux';
import GeneratedThumbnails from './GeneratedThumbnails';
import ThumbnailsGenerator from './ThumbnailsGenerator';
import ThumbsImageList from './ThumbsImageList';

interface PropType {
  video: Video;
  selectedThumbnails: string[];
  setSelectedThumbnails: (newValue: string[]) => void;
}

const ThumbsEdition = (props: PropType) => {
  const { selectedThumbnails, setSelectedThumbnails, video } = props;

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
    <Box sx={{ display: 'flex', flexDirection: 'column', flexGrow: 1 }}>
      {thumbnails?.length ? (
        <Box sx={{ flexBasis: '50%', flexGrow: 0, flexShrink: 0 }}>
          <ThumbsImageList cols={thumbnails ? Math.min(thumbnails?.length, 4) : 0} thumbs={thumbnails || []} />
        </Box>
      ) : (
        <></>
      )}
      <Box sx={{ flexBasis: '50%' }}>
        <ThumbnailsGenerator video={video} />
        {status.isLoading ? (
          <CircularProgress />
        ) : (
          <GeneratedThumbnails data={status?.data || []} selected={selectedThumbnails} onSelect={handleSelectThumdbnail}></GeneratedThumbnails>
        )}
      </Box>
    </Box>
  );
};

export default ThumbsEdition;
