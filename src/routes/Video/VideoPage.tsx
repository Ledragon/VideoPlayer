import { Box, Tab, Tabs } from '@mui/material';
import { useState } from 'react';
import { useSelector } from 'react-redux';
import { useLocation, useParams, useSearchParams } from 'react-router-dom';

import useGetVideoTags from '../../hooks/useGetVideoTags';
import { Video } from '../../models/Video';
import { getVideo } from '../../redux';
import { RootState } from '../../redux/configureStore';
import ContactSheet from '../Videos/ContactSheet';
import VideoInfos from '../Videos/VideoInfos';

// import ContactSheet from './ContactSheet';
// import VideoInfos from './VideoInfos';

const VideoPage = (props: {}) => {
  const location = useLocation();

  const urlParams = useParams();
  const id = urlParams['id'];
  const [selectedTab, setSelectedTab] = useState<number>(0);
  const video = id && useSelector((state: RootState) => getVideo(state, +id));

  const tagsForVideo = useGetVideoTags(id ? +id : undefined);
  return (
    <Box sx={{ display: 'flex', flexGrow: 1, flexDirection: 'row', overflow: 'auto' }}>
      <Box sx={{ flexBasis: '30%', flexGrow: 0, flexShrink: 0, display: 'flex', flexDirection: 'column' }}>
        <Tabs value={selectedTab} onChange={(e, v) => setSelectedTab(v)}>
          <Tab label="Properties" />
          <Tab label="Edition" />
          <Tab label="Thumbs" />
        </Tabs>
        <Box sx={{ flexGrow: 1, overflow: 'auto' }}>
          <Box sx={{ height: '100%', display: 'flex', flexDirection: 'column', overflow: 'auto' }}>
            {selectedTab === 0 && video && <VideoInfos tags={tagsForVideo} video={video} />}
            {/* {selectedTab === 1 && (
              <VideoEdtion
                error={error}
                formik={formik}
                tags={tags}
                tagVideos={tagVideos}
                video={video}
                onSave={handleSave}
                selectedThumbnails={selectedThumbnails}
                setSelectedThumbnails={setSelectedThumbnails}
              />
            )} */}
            {/* {selectedTab === 2 && <ThumbsEdition video={video} selectedThumbnails={selectedThumbnails} setSelectedThumbnails={setSelectedThumbnails} />} */}
          </Box>
        </Box>
      </Box>
      <Box sx={{ p: 1, display: 'flex', flexDirection: 'column', flexGrow: 1, flexBasis: 'auto', overflow: 'hidden' }} className="center">
        {id && <ContactSheet id={+id}></ContactSheet>}
      </Box>
    </Box>
  );
};

export default VideoPage;
