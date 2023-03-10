import { Box, LinearProgress } from '@mui/material';
import { useEffect, useState } from 'react';
import { Tag } from '../../models/Tag';
import { Video } from '../../models/Video';
import {
  useGetVideosQuery,
  useGetThumbnailsQuery,
  useGetTagsQuery,
  useGetTagVideosQuery,
  useUpdateVideoMutation
} from '../../redux';
import TagChips from './TagChips';
import VideoDialog from './VideoDialog';
import VideosImageList from './VideosImageList';

interface PropType {}
const VideosNewPage = (props: PropType) => {
  const [selectedVideo, setSelectedVideo] = useState<Video | null>(null);
  const [selectedTags, setSelectedTags] = useState<Tag[]>([]);
  const [filteredVideos, setFilteredVideos] = useState<Video[]>([]);

  const { data: videos, isLoading } = useGetVideosQuery();
  const { data: thumbnails, isLoading: isLoadingThumbnails } =
    useGetThumbnailsQuery();
  const { data: tags, isLoading: isLoadingTags } = useGetTagsQuery();
  const { data: tagVideos, isLoading: isLoadingTagVideos } =
    useGetTagVideosQuery();
  const [updateVideo, result] = useUpdateVideoMutation();

  useEffect(() => {
    setFilteredVideos(videos || []);
  }, [videos]);

  const handleVideoClick = (video: Video) => {
    if (video) {
      setSelectedVideo(video);
    }
  };

  const handleTagClick = (tag: Tag) => {
    let newSelectedTags: Tag[];
    if (selectedTags.indexOf(tag) < 0) {
      newSelectedTags = selectedTags.concat([tag]);
    } else {
      newSelectedTags = selectedTags.map((d) => d);
      newSelectedTags.splice(selectedTags.indexOf(tag), 1);
    }
    setSelectedTags(newSelectedTags);
    if (newSelectedTags.length) {
      const st = newSelectedTags.map((d) => d.id);
      const ftv =
        tagVideos
          ?.filter((d) => st.indexOf(d.tagId) >= 0)
          .map((d) => d.videoId) || [];
      const fv = videos?.filter((v) => ftv.indexOf(v.id) >= 0);
      setFilteredVideos(fv || []);
    } else {
      setFilteredVideos(videos || []);
    }
  };

  const handleSave = (video: Video) => {
    updateVideo(video)
      .unwrap()
      .then(() => setSelectedVideo(null));
  };

  const handleCancel = () => {
    setSelectedVideo(null);
  };

  const isLoadingAny =
    isLoading || isLoadingTagVideos || isLoadingTags || isLoadingThumbnails;
  return (
    <Box
      sx={{ display: 'flex', flexDirection: 'column', p: 1, height: '100%' }}
    >
      {isLoadingAny ? (
        <Box>
          <LinearProgress></LinearProgress>
        </Box>
      ) : (
        <></>
      )}
      <Box id="tags-chips">
        <TagChips
          selectedTags={selectedTags}
          tags={tags || []}
          onTagClick={handleTagClick}
        ></TagChips>
      </Box>
      <Box>
        <VideosImageList
          videos={filteredVideos}
          thumbnails={thumbnails || []}
          onClick={handleVideoClick}
        />
      </Box>
      {!!selectedVideo ? (
        <VideoDialog
          open={!!selectedVideo}
          tags={tags || []}
          tagVideos={(tagVideos || []).filter(
            (v) => v.videoId === selectedVideo?.id
          )}
          thumbnails={(thumbnails || []).filter(
            (t) => t.videoId === selectedVideo?.id
          )}
          video={selectedVideo}
          onSave={handleSave}
          onCancel={handleCancel}
        />
      ) : (
        <></>
      )}
    </Box>
  );
};

export default VideosNewPage;
