import { ActionIcon, Badge, Card, Group, Image, Stack, Text, Tooltip } from '@mantine/core';
import { IconEyeOff, IconPlaylistAdd, IconPlaylistX } from '@tabler/icons-react';
import { timeFormat } from 'd3-time-format';
import { useMemo, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Video } from '../../models/Video';
import { useGetContactSheetForVideoQuery, useGetThumbnailsForVideoQuery } from '../../redux';
import { RootState } from '../../redux/configureStore';
import { addVideo, removeVideo } from '../../redux/playlist.slice';
import { notifications } from '@mantine/notifications';
import getBase64Url from './getBase64Url';

const tf = timeFormat('%Y-%m-%d');

interface PropType {
  showContactSheet?: boolean;
  video: Video;
  onInfoClick?: (video: Video) => void;
}

const VideoImageItem = ({ video, onInfoClick, showContactSheet }: PropType) => {
  const { data: thumbs } = useGetThumbnailsForVideoQuery(video.id);
  const [hovered, setHovered] = useState<boolean>(false);
  const { data: contactSheet } = useGetContactSheetForVideoQuery(video.id);
  const dispatch = useDispatch();

  // Check if video is in playlist
  const playlistVideos = useSelector((state: RootState) => state.playlist.videos);
  const isInPlaylist = playlistVideos.includes(video.id);

  const onInfoButtonClick = (e: React.MouseEvent) => {
    e.stopPropagation();
    if (onInfoClick) {
      onInfoClick(video);
    }
  };

  const onPlaylistToggle = (e: React.MouseEvent) => {
    e.stopPropagation();

    if (isInPlaylist) {
      dispatch(removeVideo(video.id));
      notifications.show({
        title: 'Removed from playlist',
        message: `${video.title} has been removed from your playlist`,
        color: 'orange',
        position: 'bottom-right',
        autoClose: 2000
      });
    } else {
      dispatch(addVideo(video.id));
      notifications.show({
        title: 'Added to playlist',
        message: `${video.title} has been added to your playlist`,
        color: 'green',
        position: 'bottom-right',
        autoClose: 2000
      });
    }
  };

  const handleMouseEnter = () => {
    setHovered(true);
  };

  const handleMouseLeave = () => {
    setHovered(false);
  };

  const image = useMemo(() => {
    if (!showContactSheet) {
      return thumbs?.length ? thumbs[0].image : undefined;
    } else {
      return contactSheet ? contactSheet.image : undefined;
    }
  }, [thumbs, showContactSheet, contactSheet]);

  const formattedDate = useMemo(() => {
    const date = new Date(video.dateAdded);
    return tf(date);
  }, [video]);

  return (
    <Card
      shadow="sm"
      padding={0}
      radius="md"
      withBorder
      style={{
        cursor: 'pointer',
        transition: 'transform 0.2s ease, box-shadow 0.2s ease',
        transform: hovered ? 'translateY(-2px)' : 'translateY(0)',
        boxShadow: hovered ? 'var(--mantine-shadow-md)' : 'var(--mantine-shadow-sm)'
      }}
      onMouseEnter={handleMouseEnter}
      onMouseLeave={handleMouseLeave}
      onClick={onInfoButtonClick}
      id={`video-${video.id}`}>
      <Card.Section style={{ position: 'relative' }}>
        <Image src={image ? getBase64Url(image) : '/no-image-14600.svg'} height={200} alt={video.title} fit="cover" fallbackSrc="/no-image-14600.svg" />

        {/* Playlist Toggle - Always Visible */}
        <Tooltip label={isInPlaylist ? 'Remove from playlist' : 'Add to playlist'}>
          <ActionIcon
            variant="filled"
            color={isInPlaylist ? 'blue' : 'gray'}
            size="md"
            radius="xl"
            onClick={onPlaylistToggle}
            style={{
              position: 'absolute',
              top: 8,
              left: 8,
              backgroundColor: isInPlaylist ? 'rgba(34, 139, 230, 0.9)' : 'rgba(108, 117, 125, 0.8)',
              backdropFilter: 'blur(4px)',
              transition: 'all 0.2s ease'
            }}>
            {isInPlaylist ? <IconPlaylistX size={16} /> : <IconPlaylistAdd size={16} />}
          </ActionIcon>
        </Tooltip>

        {/* Duration Badge */}
        <Badge
          variant="filled"
          color="dark"
          size="sm"
          style={{
            position: 'absolute',
            bottom: 8,
            right: 8,
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            backdropFilter: 'blur(4px)'
          }}>
          {video.lengthString || video.length}
        </Badge>

        {/* Contact Sheet Status */}
        {!contactSheet && (
          <Tooltip label="No contact sheet available">
            <div style={{ position: 'absolute', top: 8, right: 8 }}>
              <IconEyeOff size={18} color="rgba(255, 255, 255, 0.7)" />
            </div>
          </Tooltip>
        )}
      </Card.Section>

      <Stack gap="xs" p="sm">
        <div>
          <Text size="sm" fw={600} lineClamp={2} title={video.title}>
            {video.title}
          </Text>
        </div>

        <Group justify="space-between" align="center">
          <Text size="xs" c="dimmed">
            {video.numberOfViews} views
          </Text>

          <Text size="xs" c="dimmed">
            {formattedDate}
          </Text>
        </Group>
      </Stack>
    </Card>
  );
};

export default VideoImageItem;
